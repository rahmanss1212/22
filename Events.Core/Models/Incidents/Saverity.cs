using Events.Core.Models;
using Events.Core.Models.General;

namespace Events.Api.Models.Incidents
{
    public class Saverity : Model,IConstant
    {
   
        public string Code { get; set; }
        public string Lable { get; set; }
    }
}