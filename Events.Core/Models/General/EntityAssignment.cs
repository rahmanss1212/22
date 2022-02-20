using Events.Api.Models.UserManagement;
using System;

namespace Events.Core.Models.General
{
    public class EntityAssignment : MainModel
    {
        public EUser User { get; set; }
        public long? UserId { get; set; }
        public EntityStatus Status { get; set; }
        public string Request { get; set; }
        public bool IsHandeled { get; set; }
        public long? IncidentId { get; set; }
        public string Response { get; set; }
         
    }
}
