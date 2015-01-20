using Autofac;
using Autofac.Core;
using System;
using System.Linq;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using System.Collections.Generic;

namespace Blacklite.Framework.Multitenancy.Autofac
{
    public class NullMatchingLifetimeScope : IComponentLifetime
    {
        readonly object[] _tagsToMatch;
        public NullMatchingLifetimeScope(params object[] lifetimeScopeTagsToMatch)
        {
            if (lifetimeScopeTagsToMatch == null) throw new ArgumentNullException("lifetimeScopeTagsToMatch");

            _tagsToMatch = lifetimeScopeTagsToMatch;
        }

        public ISharingLifetimeScope FindScope(ISharingLifetimeScope mostNestedVisibleScope)
        {
            if (mostNestedVisibleScope == null) throw new ArgumentNullException("mostNestedVisibleScope");

            var next = mostNestedVisibleScope;
            while (next != null)
            {
                if (_tagsToMatch.Contains(next.Tag))
                    return next;

                next = next.ParentLifetimeScope;
            }

            return new EmptySharingLifetimeScope();
        }
    }

    class EmptySharingLifetimeScope : ISharingLifetimeScope
    {
        public EmptySharingLifetimeScope()
        {
            RootLifetimeScope = this;
        }

        IComponentRegistry IComponentContext.ComponentRegistry
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        IDisposer ILifetimeScope.Disposer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ISharingLifetimeScope ISharingLifetimeScope.ParentLifetimeScope { get; } = null;

        public ISharingLifetimeScope RootLifetimeScope { get; }

        object ILifetimeScope.Tag { get; } = null;

        event EventHandler<LifetimeScopeBeginningEventArgs> ILifetimeScope.ChildLifetimeScopeBeginning
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<LifetimeScopeEndingEventArgs> ILifetimeScope.CurrentScopeEnding
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<ResolveOperationBeginningEventArgs> ILifetimeScope.ResolveOperationBeginning
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        ILifetimeScope ILifetimeScope.BeginLifetimeScope()
        {
            throw new NotImplementedException();
        }

        ILifetimeScope ILifetimeScope.BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            throw new NotImplementedException();
        }

        ILifetimeScope ILifetimeScope.BeginLifetimeScope(object tag)
        {
            throw new NotImplementedException();
        }

        ILifetimeScope ILifetimeScope.BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        object ISharingLifetimeScope.GetOrCreateAndShare(Guid id, Func<object> creator)
        {
            return null;
        }

        object IComponentContext.ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return null;
        }
    }
}
