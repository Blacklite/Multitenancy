using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Blacklite.Framework.Multitenancy.ApplicationEvents
{
    public class ApplicationBroadcastComposer : ITenantComposer, IDisposable
    {
        private readonly IApplicationOrchestrator _orchestrator;
        private readonly IList<IDisposable> _disposables;

        public ApplicationBroadcastComposer(IApplicationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            _disposables = new List<IDisposable>();
        }

        public int Order { get; } = 0;

        public void Configure(ITenant tenant)
        {
            _disposables.Add(
                tenant.Events
                .Select(ApplicationEvent.Create(tenant.Id))
                .Subscribe(_orchestrator.Broadcast)
            );
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
                    foreach (var d in _disposables) d.Dispose();
                    _disposables.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApplicationBroadcastConfigurator() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
