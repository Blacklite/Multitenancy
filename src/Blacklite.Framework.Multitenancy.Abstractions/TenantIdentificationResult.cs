using System;

namespace Blacklite.Framework.Multitenancy
{
    public class TenantIdentificationResult
    {
        public static TenantIdentificationResult Failed = new TenantIdentificationResult();
        public static TenantIdentificationResult Passed = new TenantIdentificationResult("Default");

        private TenantIdentificationResult()
        {
            Id = null;
            Success = false;
            Enabled = false;
        }

        public TenantIdentificationResult(string id, bool success = true, bool enabled = true)
        {
            Id = id;
            Success = success;
            Enabled = enabled;
        }

        public string Id { get; }

        public bool Success { get; }

        public bool Enabled { get; }
    }
}
