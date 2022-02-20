using System.Diagnostics.Contracts;
using Events.Api.Models.UserManagement;
using Events.Core.Models.General;

namespace Events.Core.Models.Reports
{
    public class GReportEntityAssignment : MainModel
    {
        
        public EUser User { get; set; }
        public long? UserId { get; set; }
        public Status Status { get; set; }
        public long? StatusId { get; set; }
        public string Request { get; set; }
        public bool IsHandeled { get; set; }
        public long? GeneralReportId { get; set; }
        
        //public GeneralReport GeneralReport { get; set; }
        public string Response { get; set; }
    }
}