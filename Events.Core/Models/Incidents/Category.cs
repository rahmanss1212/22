using Events.Core.Models;
using Events.Core.Models.General;

namespace Events.Api.Models.Incidents
{
    public class Category  : Model,IConstant
    {
        public string code { set; get; }
        public string label { set; get; }

    }
}