using System;
using System.Collections.Generic;

namespace Events.Core.Models.Incidents
{
    public class AssignModelIncident
    {
        public List<Int64> Users { get; set; }

        public long Incident { get; set; }

        public long AssignmentId { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

    }
}
