using Autofac;
using Autofac.Builder;
using Blacklite;
using Blacklite.Framework;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Autofac;
using Microsoft.Framework.Configuration;
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
        public static T Populate<T>([NotNull] this T builder, IServiceCollection services)
            where T : ContainerBuilder
        {
            services.AddSingleton<ITenantProvider, AutofacTenantProvider>();
            services.AddMultitenancy();

            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>();
            builder.RegisterType<AutofacServiceScopeFactory>().As<IServiceScopeFactory>();

            Register(builder, services);

            return builder;
        }

        public static IServiceProvider BuildMultitenancy<T>([NotNull] this T builder)
            where T : ContainerBuilder
        {
            var container = builder.Build();

            // We make the default application services the application container.
            var applicationLifetime = container.BeginLifetimeScope(AutofacTenantProvider.ApplicationTag);
            return applicationLifetime.Resolve<IServiceProvider>();
        }

        private static void Register(
                ContainerBuilder builder,
                IEnumerable<ServiceDescriptor> descriptors)
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
                            .ConfigureLifetime(descriptor);
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifetime(descriptor);
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureLifetime(descriptor)
                    .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .ConfigureLifetime(descriptor);
                }
            }
        }

        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> ConfigureLifetime<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
                ServiceDescriptor descriptor)
        {
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    if (descriptor.IsTenantScope())
                        registration.InstancePerTenantScope();
                    else if (descriptor.IsApplicationScope())
                        registration.InstancePerApplicationScope();
                    else // Global
                        registration.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    registration.InstancePerDependency();
                    break;
            }

            return registration;
        }
    }

}
