using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ApplicationOnlyAttribute : Attribute { }
}
