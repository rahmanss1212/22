namespace Events.Core.Models.General
{
    public class OrganizationIps:Model
    {
        public long OrgId { get; set; }
        public string IpName { get; set; }
        public string Note { get; set; }
    }
}