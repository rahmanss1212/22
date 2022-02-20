using Events.Api.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Events.Core.Models.General;

namespace Events.Core.Models.Tasks
{
    public class TaskType : Model,IConstant
    {
        public string Name { get; set; }
    }
}
