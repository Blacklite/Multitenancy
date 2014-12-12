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
            catch (DependencyResolutionException dre) if (dre.Message.Contains("\{AutofacTenantProvider.TenantTag}"))
            {
                throw new NotSupportedException("Cannot resolve dependency '\{serviceType.FullName}'.  It can only be resolved from inside of a tenant.", dre);
            }
            catch (DependencyResolutionException dre) if (dre.Message.Contains("\{AutofacTenantProvider.ApplicationTag}"))
            {
                throw new NotSupportedException("Cannot resolve dependency '\{serviceType.FullName}'.  It can only be resolved from outside of a tenant", dre);
            }
        }
    }
}
