using Events.Api.Models.UserManagement;

namespace Events.Core.Models.DPE
{
    public class DPEWorkTeam
    {
        public DPE Dpe { get; set; }
        public long? DpeId { get; set; }
        public EUser EUser { get; set; }
        public long? EUserId { get; set; }
    }
}