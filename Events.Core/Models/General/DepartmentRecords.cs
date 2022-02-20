using System;

namespace Events.Core.Models.General
{
    public class DepartmentRecords : Model
    {
        public string Name { get; set; }
        public Int32 Tasks { get; set; }
        public Int32 Incidents { get; set; }
        public Int32 Notifications { get; set; }
        public Int32 Reports { get; set; }
        public Int32 Apts { get; set; }
        public Int32 Dpes { get; set; }
    }
}