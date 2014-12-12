using Autofac.Core;
using Autofac.Core.Resolving;
using Blacklite.Framework.Multitenancy.Autofac;
using System;

namespace Autofac.Builder
{
    public static class RegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerTenantScope<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(AutofacTenantProvider.TenantTag);
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerApplicationScope<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(AutofacTenantProvider.ApplicationTag);
        }
    }
}
