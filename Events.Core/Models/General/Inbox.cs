using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Core.Models.General
{
    public class Inbox : Model
    {
        

        public int TypeId { get; set; }
        public string Type { get; set; }
        public int SubTypeId { get; set; }
        public DateTime Date { get; set; }
        public string FromUser { get; set; }
        public string Claim { get; set; }
        public long? ToUser { get; set; }
        public string Title { get; set; }
        public string Brife { get; set; }
        public bool isNew { get; set; }
        public bool isUpdated { get; set; }
        public long EntityStatusId { get; set; }
        public long FromUserId { get; set; }
    }
}