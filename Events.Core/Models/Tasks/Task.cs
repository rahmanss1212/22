using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;

namespace Events.Core.Models.Tasks
{
    public class Task : MainModel
    {
        public string Description { get; set; }
        public TaskType TaskType { get; set; }
        public long? TaskTypeId { get; set; }
        public string Title { get; set; }
        public int Importance { get; set; }
        public int Urgent { get; set; }
        public Status Status { get; set; } 
        public long StatusId { get; set; } 
        
        [NotMapped]
        public List<long> DeletedAttachments { get; set; }
        public Section Assigned_for { get; set; }
        public long? Assigned_forId { get; set; }
        public List<TaskEmpsRel> AssignedEmps { get; set; }
        public List<TaskAttachments> Attachments { get; set; }
        public int Weight { get; set; }
        public string Date { get; set; }
        public string DueDate { get; set; }
        public List<TaskComment> TaskComments { get; set; }
        public Incident ParentIncident { get; set; }
        
        public long? ParentIncidentId { get; set; }
        public CloseReport ClosingReport { get; set; }
        public Task ParentTask { get; set; }
        public long? ParentTaskId { get; set; }
        public bool IsIncident { get; set; }
        public int Progress { get; set; }
        public int Rate { get; set; }
        public List<TaskEntityAssignment> TaskAssignments { set; get; } = new List<TaskEntityAssignment>();
    }
}