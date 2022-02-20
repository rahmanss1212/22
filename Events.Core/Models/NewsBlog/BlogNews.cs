using Events.Api.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.NewsBlog
{
    public class BlogNews : MainModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public BlogNewsCategory NewsCategory { get; set; }
    }
}
