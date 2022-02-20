using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Core.Models.General;

namespace Events.Core.Models.Incidents
{
    public class Incident : MainModel
    {
        public Category Category { set; get; }
        public long? CategoryId { set; get; }
        public string Signature { set; get; }
        
        public APT Apt { set; get; }
        
        public long AptId { set; get; }
        public string Description { set; get; }
        public CloseReport CloseReport { get; set; }
        
        [NotMapped]
        public List<long> DeletedAttachments { get; set; }
        public string ExtraNote1 { get; set; }
        public String ExtraNote2 { get; set; }
        public String ExtraNote3 { get; set; }
        public EntityStatus Status { set; get; }
        public string Subject { set; get; }
        public Saverity Saverity { set; get; }
        public long? SaverityId { set; get; }
        public DateTime Date { get; set; }
        public string Time { get; set; }

        public Urgancey Urgancey { set; get; }
        
        public long? UrganceyId { set; get; }
        public List<IpAddress> IpAddresses { set; get; }
        public List<IncidentComment> Comments { set; get; }
        public List<IncidentAttachment> Attachments { set; get; }
        public List<EntityAssignment> Assignments { set; get; } = new List<EntityAssignment>();
        public List<OrgsIncidentRel> Orgs { set; get; }
        public List<Tag> Tags { set; get; }
        public bool IsIpsIdentificationRequested { get; set; }
        public IncidentCategory IncidentCategory { get; set; }
    }
}