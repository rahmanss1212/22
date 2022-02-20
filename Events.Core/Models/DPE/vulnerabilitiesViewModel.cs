using System.Collections.Generic;
using System.ComponentModel;

namespace Events.Core.Models.DPE
{
    public class vulnerabilitiesViewModel
    {
        public List<Vulnerability> Vulnerabilities { get; set; }
        public bool IsSubmit { get; set; }
        public long DpeId { get; set; }
    }
}