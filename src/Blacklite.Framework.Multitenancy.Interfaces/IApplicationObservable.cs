using Microsoft.Framework.Runtime;
using System;

namespace Blacklite.Framework.Multitenancy
{
    [AssemblyNeutral]
    [ApplicationOnly]
    public interface IApplicationObservable : IObservable<IApplicationEvent>
    {

    }
}
