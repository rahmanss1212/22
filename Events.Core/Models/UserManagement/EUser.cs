using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Events.Core.Models.DPE;

namespace Events.Api.Models.UserManagement
{
    public class EUser : IdentityUser<long>
    {
        public string FullName { get; set; }

        public Section Section { get; set; }
        public long? SectionId { get; set; }
        
        public IList<TaskEmpsRel> Tasks { get; set; }
        
        public IList<DPEWorkTeam> DpeWorkTeam { set; get; }

        public bool IsEnabled { get; set; }

        public bool IsHead { get; set; }

        public bool IsSubHead { get; set; }

        public bool IsAssignedHead { get; set; }

        public bool IsAssignedSubeHead { get; set; }
    }
}
