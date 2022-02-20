using Events.Api.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.DPE
{
    public class DPEAttachment
    {
        public long AttachmentId { get; set; }
        public Attachment Attachment { get; set; }
        public long DPEId { get; set; }
        public Vulnerability Vulnerability { get; set; }
    }
}
