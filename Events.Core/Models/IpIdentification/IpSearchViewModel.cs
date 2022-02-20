using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.IpIdentification
{
    public class IpSearchViewModel
    {

        public String Apts { get; set; }
        public String Owners { get; set; }
        
        public String OwnerType { get; set; }
        public String Search { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}
