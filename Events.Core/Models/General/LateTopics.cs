namespace Events.Core.Models.General
{
    public class LateTopics : Model
    {
        public string Name { get; set; }
        public int Tasks { get; set; }
        public int Incidents { get; set; }
        public int Vulnerability { get; set; }
        public int IpAddress { get; set; }
        public int lateInforming { get; set; }
        public int Reports { get; set; }
    }
}