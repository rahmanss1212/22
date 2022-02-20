using System;

namespace Events.Core.Models.APTs
{
    public class AptView : Model
    {
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public string Targeted { get; set; }
        public string Origin { get; set; }
        
        
    }
}