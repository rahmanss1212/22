using Events.Api.Models.UserManagement;
using Events.Core.Models;



namespace Events.Api.Models.General
{
    public class NewsBar : Model
    {
        public string title { set; get; }
        public string description { set; get; }
        public int depId { set; get; }
        public bool publish { set; get; }
        public EUser Users { get; set; }
        public Urgancey Urgancey { set; get; }

    }
}
