using Events.Api.Models.UserManagement;
using Events.Core.Models.General;

namespace Events.Core.Models.Tasks
{
    public class TaskEntityAssignment : MainModel
    {
        public EUser User { get; set; }
        public long? UserId { get; set; }
        public EntityStatus Status { get; set; }
        public string Request { get; set; }
        public bool IsHandled { get; set; }
        public long? TaskId { get; set; }
        public string Response { get; set; }
    }
}