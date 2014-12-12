﻿using System;

namespace Blacklite.Framework.Multitenancy.Events
{
    public class Event
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Event Clone()
        {
            return (Event)this.MemberwiseClone();
        }

        public static Event Boot() { return new Event() { Type = "\{TenantState.Boot}" }; }
        public static Event Start() { return new Event() { Type = "\{TenantState.Started}" }; }
        public static Event Stop() { return new Event() { Type = "\{TenantState.Stopped}" }; }
        public static Event Shutdown() { return new Event() { Type = "\{TenantState.Shutdown}" }; }
    }
}
