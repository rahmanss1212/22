using Events.Api.Models.UserManagement;
using System;
using Events.Core.Models;

namespace Events.Api.Models.APTs
{
    public class Content : Model
    {

        public string ContentString { set; get; }
        public EUser CreatedBy { set; get; }
        public long CreatedById { set; get; }
        public DateTime createdDate { set; get; } = DateTime.Now;

    }
}