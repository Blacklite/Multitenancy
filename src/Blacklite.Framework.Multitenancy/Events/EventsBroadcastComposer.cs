using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using Blacklite.Framework.Multitenancy.Events;

namespace Blacklite.Framework.Multitenancy.Events
{
    public class EventsBroadcastComposer : ITenantComposer, IDisposable
    {
        private readonly CompositeDisposable _disposable;

        public EventsBroadcastComposer()
        {
            _disposable = new CompositeDisposable();
        }

        public int Order { get; } = 0;

        public void Configure(ITenant tenant, IServiceProvider tenantServiceProvider)
        {
            _disposable.Add(MultitenancyEvents.Global.Add(
                tenant.Events.Select(z => TenantEvent.Create(tenant.Id, z))
            ));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _disposable.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
