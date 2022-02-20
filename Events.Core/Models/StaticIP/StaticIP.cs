using System;

namespace Events.Core.Models.StaticIP
{
    public class StaticIP : Model
    {
        public string Name { get; set; }
        public long Mobile { get; set; }
        public string Ip { get; set; }
        public DateTime EditDate { get; set; }
        public string Provider { get; set; }
        public bool IsActive { get; set; } = true;
    }
}