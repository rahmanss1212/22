using Events.Api.Models.APTs;
using Events.Api.Models.General;
using System.ComponentModel.DataAnnotations;
using Events.Core.Models.General;

namespace Events.Core.Models.APTs
{
    public class TargetedSector
    {
        public long SectorId { get; set; }
        public Sector Sector { get; set; }
        public long AptId { get; set; }
        public APT Apt { get; set; }
    }
}
