using System.Collections.Generic;

namespace Events.Core.Models.Tasks
{
    public class AssignModelTask
    {
        public List<long> Users { get; set; }

        public long Task { get; set; }

        public long AssignmentId { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }
    }
}
