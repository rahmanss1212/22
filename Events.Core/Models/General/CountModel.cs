using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.General
{
    public class CountModel : Model
    {
        //incidents
        public long IncidentsISoc { get; set; }
        public long IncidentsESec { get; set; }

        //ips
        public long VarifyRequests { get; set; }

        //inbox
        public long inbox { get; set; }

        //notifications
        public long Notifications { get; set; }

        //APT 
        public long aptISoc { get; set; }
        public int aptESec { get; set; }
        public int WaitingOrgInformVul { get; set; }
        public int WaitingFixVul { get; set; }
        public int WaitingFixAuthVul { get; set; }
        public int LateVul { get; set; }
        public List<DepartmentRecords> DepartmentRecords { get; set; }
        public int VulMedium { get; set; }
        public int VulCritical { get; set; }
        public int VulLow { get; set; }
        public int Reports { get; set; }
        public int Incidents { get; set; }
        public object TasksPrgressAverage { get; set; }
        public List<LateTopics> LateTopics { get; set; }
    }
}