using Events.Api.Models.General;
using Events.Core.Models.General;

namespace Events.Core.Models.Reports
{
    public class ReportCategory : Model,IConstant
    {
        public string Title { get; set; }
    }
}