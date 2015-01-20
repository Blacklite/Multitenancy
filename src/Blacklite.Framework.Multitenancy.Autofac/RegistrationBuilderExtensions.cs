using Autofac.Core;
using Autofac.Core.Resolving;
using Blacklite.Framework.Multitenancy.Autofac;
using System;

namespace Autofac.Builder
{
    public static class RegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerNullMatchingLifetimeScope<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, params object[] lifetimeScopeTag)
        {
            builder.InstancePerMatchingLifetimeScope(lifetimeScopeTag);
            builder.RegistrationData.Lifetime = new NullMatchingLifetimeScope(lifetimeScopeTag);
            return builder;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerTenantScope<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerNullMatchingLifetimeScope(AutofacTenantProvider.TenantTag);
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerApplicationScope<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerNullMatchingLifetimeScope(AutofacTenantProvider.ApplicationTag);
        }
    }
}
