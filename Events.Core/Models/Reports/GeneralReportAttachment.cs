

using Events.Api.Models.General;

namespace Events.Core.Models.Reports
{
    public class GeneralReportAttachment
    {
        public long GeneralReportId { get; set; }  
        public long AttachmentId { get; set; }
        public GeneralReport GeneralReport { get; set; }
        public Attachment Attachment { get; set; }
    }
}