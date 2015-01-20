using Autofac;
using Autofac.Builder;
using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Autofac;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac
{
    public static class AutofacTenantRegistration
    {
        public static T Populate<T>(
                [NotNull] this T builder,
                IServiceCollection services,
                IConfiguration configuration = null)
            where T : ContainerBuilder
        {
            services.AddSingleton<ITenantProvider, AutofacTenantProvider>();
            services.AddMultitenancy(configuration);

            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>();
            builder.RegisterType<AutofacServiceScopeFactory>().As<IServiceScopeFactory>();

            Register(builder, services);

            return builder;
        }

        public static IServiceProvider BuildMultitenancy<T>([NotNull] this T builder)
            where T : ContainerBuilder
        {
            var container = builder.Build();
            var applicationLifetime = container.BeginLifetimeScope(AutofacTenantProvider.ApplicationTag);
            return applicationLifetime.Resolve<IServiceProvider>();
        }

        private static void Register(
                ContainerBuilder builder,
                IEnumerable<IServiceDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    // Test if the an open generic type is being registered
                    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor);
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor);
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureLifecycle(descriptor)
                    .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor);
                }
            }
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> ConfigureLifecycle<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
                IServiceDescriptor descriptor)
        {
            switch (descriptor.Lifecycle)
            {
                case LifecycleKind.Singleton:
                    if (descriptor.IsTenantScope())
                        registration.InstancePerTenantScope();
                    else if (descriptor.IsApplicationScope())
                        registration.InstancePerApplicationScope();
                    else // Global
                        registration.SingleInstance();
                    break;
                case LifecycleKind.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case LifecycleKind.Transient:
                    registration.InstancePerDependency();
                    break;
            }

            return registration;
        }
    }

}
