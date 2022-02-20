using Events.Core.Models.General;
using System.Collections.Generic;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;

namespace Events.Core.Models.Reports
{
    public class GeneralReport : MainModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Status Status { get; set; }
        public long StatusId { get; set; }
        public long CreatedById { get; set; }

        public List<Tag> Tags { get; set; }

        public List<GeneralReportAttachment> Attachments { get; set; }
        public ReportCategory ReportCategory { get; set; }
        public long ReportCategoryId { get; set; }

        public List<GReportEntityAssignment> Assignments { set; get; } = new List<GReportEntityAssignment>();
        public Urgancey Urgancey { set; get; }

        public long? UrganceyId { set; get; }
        public Saverity Saverity { set; get; }
        public long? SaverityId { set; get; }
    }
}