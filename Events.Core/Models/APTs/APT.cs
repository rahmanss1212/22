
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.APTs;
using Events.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Events.Core.Models.General;

namespace Events.Api.Models.APTs
{
    public class APT : MainModel
    {

        public string Name { set; get; }
        public List<TargetedCountry> Targeted { set; get; }
        public List<OriginCountry> Origin { set; get; }
        public List<Content> Contents { set; get; }
        public List<ThreatSignature> ThreatSignatures { set; get; }
        public List<AlternativeName> AlternativeNames { set; get; }
        public List<AttackStratigie> AttackStratigies { set; get; }
        public Status Status { get; set; }
        public long? StatusId { get; set; }
        public List<AptAttachment> Attachments { set; get; }
        public IList<CompanyName> CompanyNames { set; get; }
        public IList<TargetedSector> TargetSectorNames { set; get; }
        public IList<ToolName> ToolsNames { set; get; }

        public List<ChangeLog> Changes { get ; set; }
    }

    public class APTView
    {
        public long Id { get; set; }
        public String Name { get; set; }
       
    }


}