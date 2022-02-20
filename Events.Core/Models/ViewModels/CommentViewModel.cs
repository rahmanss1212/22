using Events.Api.Models.General;
using Events.Api.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Events.Core.Models.Tasks;

namespace Events.Core.Models.ViewModels
{
    public class CommentViewModel
    {
        public int relid { get; set; }
        public String commentString { get; set; }
        public int parentCommentId { get; set; }

        public List<Attachment> attachments { get; set; }

        public static implicit operator CommentViewModel(Task v)
        {
            throw new NotImplementedException();
        }
    }
}
