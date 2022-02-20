using Events.Core.Models;
using Events.Core.Models.General;

namespace Events.Api.Models.General
{
    public class Urgancey : Model,IConstant
    {
        public string Code { set; get; }
        public string Label { set; get; }
    }
}