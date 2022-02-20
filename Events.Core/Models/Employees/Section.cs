using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.Employees;
using System.Collections.Generic;
using Events.Core.Models.General;
using Events.Core.Models.Tasks;

namespace Events.Api.Models.General
{
    public class Section : Model,IConstant
    {
        public string Name { get; set; }

        public Department Department { get; set; }
        public long? DepartmentId { get; set; }

        public IList<EUser> Users { get; set; }

        public IList<Task> Tasks { get; set; }

    }
}