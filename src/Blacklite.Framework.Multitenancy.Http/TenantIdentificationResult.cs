using System;

namespace Blacklite.Framework.Multitenancy.Http
{
    public interface ITenantIdentificationResult
    {
        string Id { get; }
        bool Success { get; }
    }

    public class TenantIdentificationResult : ITenantIdentificationResult
    {
        public static ITenantIdentificationResult Failed = new TenantIdentificationResult();
        public static ITenantIdentificationResult Passed = new TenantIdentificationResult("Default");

        private TenantIdentificationResult()
        {
            Id = null;
            Success = false;
        }

        public TenantIdentificationResult(string id, bool success = true)
        {
            Id = id;
            Success = success;
        }

        public string Id { get; }

        public bool Success { get; }
    }
}
