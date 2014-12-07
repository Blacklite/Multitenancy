using Blacklite.Framework.Multitenancy.ConfigurationModel;
using Blacklite.Framework.Multitenancy.Events;
using Blacklite.Framework.Multitenancy.Operations;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blacklite.Framework.Multitenancy
{
    public enum TenantState
    {
        Boot,
        Started,
        Stopped,
        Shutdown
    }

    public interface ITenant : IDisposable
    {
        void Initialize([NotNull] string identifier);
        string Id { get; }
        TenantState State { get; }
        IConfiguration Configuration { get; }

        event EventHandler<OnBootEventArgs> OnBoot;
        event EventHandler<OnStartEventArgs> OnStart;
        event EventHandler<OnStopEventArgs> OnStop;
        event EventHandler<OnShutdownEventArgs> OnShutdown;
    }

    [LifecyclePerTenant]
    public class Tenant : ITenant
    {
        private bool _initalized = false;

        public Tenant(ITenantConfiguration configuration)
        {
            Configuration = configuration;

            OnBoot += (object sender, OnBootEventArgs e) => this.State = TenantState.Boot;
            OnStart += (object sender, OnStartEventArgs e) => this.State = TenantState.Started;
            OnStop += (object sender, OnStopEventArgs e) => this.State = TenantState.Stopped;
            OnShutdown += (object sender, OnShutdownEventArgs e) => this.State = TenantState.Shutdown;
        }

        public void Initialize(string identifier)
        {
            if (_initalized)
                return;

            Id = identifier;

            RaiseOnBoot(new OnBootEventArgs());

            _initalized = true;
        }

        public string Id { get; private set; }

        public IConfiguration Configuration { get; }
        
        public TenantState State { get; private set; }

        public event EventHandler<OnBootEventArgs> OnBoot;

        private void RaiseOnBoot(OnBootEventArgs e) { if (OnBoot != null) OnBoot(this, e); }

        internal bool ExecuteBootOperation(BootOperation context)
        {
            RaiseOnBoot(new OnBootEventArgs());
            return true;
        }

        public event EventHandler<OnStartEventArgs> OnStart;

        private void RaiseOnStart(OnStartEventArgs e) { if (OnStart != null) OnStart(this, e); }

        internal bool ExecuteStartOperation(StartOperation context)
        {
            RaiseOnStart(new OnStartEventArgs());
            return true;
        }

        public event EventHandler<OnStopEventArgs> OnStop;

        private void RaiseOnStop(OnStopEventArgs e) { if (OnStop != null) OnStop(this, e); }

        internal bool ExecuteStopOperation(StopOperation context)
        {
            RaiseOnStop(new OnStopEventArgs());
            return true;
        }

        public event EventHandler<OnShutdownEventArgs> OnShutdown;

        private void RaiseOnShutdown(OnShutdownEventArgs e) { if (OnShutdown != null) OnShutdown(this, e); }

        internal bool ExecuteShutdownOperation(ShutdownOperation context)
        {
            RaiseOnShutdown(new OnShutdownEventArgs());
            return true;
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
                    if (State == TenantState.Started)
                    {
                        ExecuteStopOperation(new StopOperation());
                    }

                    if (State == TenantState.Stopped)
                    {
                        ExecuteShutdownOperation(new ShutdownOperation());
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                OnBoot = null;
                OnStart = null;
                OnStop = null;
                OnShutdown = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}