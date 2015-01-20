#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
#endif
using System;

namespace Blacklite.Framework.Multitenancy
{
#if ASPNET50 || ASPNETCORE50
    [AssemblyNeutral]
#endif
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ApplicationOnlyAttribute : Attribute { }
}
