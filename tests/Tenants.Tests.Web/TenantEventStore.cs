using Blacklite.Framework.Events;
using Blacklite.Framework.Multitenancy;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Collections;
using Newtonsoft.Json;

namespace Tenants.Tests.Web
{
    public class TenantEventStoreComposer : ITenantComposer, IDisposable
    {
        private readonly IList<IDisposable> _disposables;

        public TenantEventStoreComposer()
        {
            _disposables = new List<IDisposable>();
        }

        public int Order { get; } = 0;

        public void Configure(ITenant tenant)
        {
            var eventStore = tenant.Services.GetService<TenantEventStore>();

            _disposables.Add(
                tenant.Events
                .Select(JsonConvert.SerializeObject)
                .Subscribe(eventStore.Add)
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

    public class TenantEventStore : IEnumerable<string>
    {
        private readonly IList<string> _items = new List<string>();

        public void Add(string item) => _items.Add(item);

        public IEnumerator<string> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
