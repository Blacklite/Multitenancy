using System;

namespace Blacklite.Framework.Multitenancy.Operations
{
    public class Operation
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Operation Clone()
        {
            return (Operation)this.MemberwiseClone();
        }

        public static Operation Boot() { return new Operation() { Type = "\{TenantState.Boot}" }; }
        public static Operation Start() { return new Operation() { Type = "\{TenantState.Started}" }; }
        public static Operation Stop() { return new Operation() { Type = "\{TenantState.Stopped}" }; }
        public static Operation Shutdown() { return new Operation() { Type = "\{TenantState.Shutdown}" }; }
    }
}
