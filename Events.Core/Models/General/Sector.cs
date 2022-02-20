using System.Collections.Generic;
using Events.Api.Models.General;

namespace Events.Core.Models.General
{
    public class Sector: Model
    {
        public string Name { get; set; }
        public IList<Organization> Organizations { get; set; }
    }
}
