using System;

namespace Microsoft.Framework.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class LifecyclePerTenantAttribute : Attribute { }
}