using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.DPE
{
    public class DPEview : Model
    {
        public DateTime LastUpdateDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long OrganizationId { set; get; }
        public int VulnerCount { set; get; }
        public long? Severity { set; get; }
        public long StatusId { set; get; }
        public long? VulnerStatus { set; get; }
        public long AssessmentMethodologyId { set; get; }
        public string Orgname { set; get; }
        public int? RemainingDays { set; get; }
        public string Title { set; get; }
    }
}
