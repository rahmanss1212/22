using System;

namespace Events.Core.Models.Reports
{
    public class GeneralReportDataView : Model
    {
        public string CreatedByName { get; set; }
        public string Title { get; set; }
        
        public long StatusId { get; set; }
        
        public string Status { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryTitle { get; set; }
    }
}
