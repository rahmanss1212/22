using System;
using System.Collections.Generic;
using System.Text;
using Events.Core.Models.General;
using Events.Api.Models.General;
using Events.Api.Models.UserManagement;

namespace Events.Core.Models.DPE
{
    public class DPE : MainModel
    {
        public IList<Vulnerability> Vulnerability { set; get; }
        public string InformingBy { get; set; }
        public string InformingNote { get; set; }

        public IList<DPEWorkTeam> DpeWorkTeam { set; get; }
        public Organization Organization { set; get;}
        
        public long? OrganizationId { set; get;}

        public OrganizationContact ContactedPerson { get; set; }
        public long? ContactedPersonId { get; set; }

        public string MethodOfInforming { get; set; }
        
        public string Recommendation { get; set; }
        
        public string Title { get; set; }
        
        public string HeadDirections { get; set; }
        
        public DateTime InformingDate { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        

        public Status Status { get; set; }
        
        public long? StatusId { get; set; }

        public string Scope { set; get; }
        public AssessmentMethodology AssessmentMethodology { set; get; }     
        
        public long? AssessmentMethodologyId { set; get; }     
        
        public Attachment AttachmentReport { set; get; }
        public bool IsOpen { set; get; }
        public bool IsDone { set; get; }
        public string ExtraNote1 { set; get; }
        

    }
}
