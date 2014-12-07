﻿using Autofac;
using Autofac.Builder;
using Blacklite;
using Blacklite.Framework.Multitenancy;
using Blacklite.Framework.Multitenancy.Autofac;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.Autofac;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autofac
{
    public static class AutofacTenantRegistration
    {
        internal static string TenantTag = "Tenant";

        public static T PopulateMultitenancy<T>(
                [NotNull] this T builder,
                IServiceCollection services,
                IConfiguration configuration = null)
            where T : ContainerBuilder
        {
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddMultitenancy(configuration);

            MultitenancyServices.HasRequiredServicesRegistered(services);

            AutofacRegistration.Populate(builder, services.Where(MultitenancyServices.IsNotTenantSingleton));

            builder.Register(x => x.Resolve<ITenantLogger>())
                .As<ILogger>()
                .InstancePerMatchingLifetimeScope(TenantTag);

            Register(builder, services.Where(MultitenancyServices.IsTenantSingleton));

            return builder;
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
                            .ConfigureLifecycle(descriptor.Lifecycle);
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifecycle);
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureLifecycle(descriptor.Lifecycle)
                    .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifecycle);
                }
            }
        }

        private static IRegistrationBuilder<object, T, U> ConfigureLifecycle<T, U>(
                this IRegistrationBuilder<object, T, U> registrationBuilder,
                LifecycleKind lifecycleKind)
        {
            switch (lifecycleKind)
            {
                case LifecycleKind.Singleton:
                    registrationBuilder.InstancePerMatchingLifetimeScope(TenantTag);
                    break;
                case LifecycleKind.Scoped:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case LifecycleKind.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            return registrationBuilder;
        }
    }

}
