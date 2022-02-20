namespace Events.Core.Models.General
{
    public class VulnerClassCount
    {
        private int Critical { get; set; }
        private int Medium{ get; set; }
        private int Low{ get; set; }

        public VulnerClassCount()
        {
            
        }

        public VulnerClassCount(int critical, int medium, int low)
        {
            Critical = critical;
            Medium = medium;
            Low = low;
        }
    }
}