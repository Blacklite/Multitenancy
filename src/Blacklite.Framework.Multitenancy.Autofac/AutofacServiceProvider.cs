using Autofac;
using Autofac.Core;
using System;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    class AutofacServiceProvider : IServiceProvider
    {
        private readonly IComponentContext _componentContext;

        public AutofacServiceProvider(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _componentContext.ResolveOptional(serviceType);
            }
            catch
            {
                // Service provider interfaces says if it can't be resolved, it returns null, does not throw.
                return null;
            }
        }
    }
}
