using System.ComponentModel.DataAnnotations;
using Events.Core.Models;

namespace Events.Api.Models.APTs
{
    public class ThreatSignature : Model
    {
        public int Serial { set; get; }
        public string Name { set; get; }
        public string DomainName { set; get; }
    }

}
