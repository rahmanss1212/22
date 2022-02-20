﻿using Events.Api.Models.UserManagement;
using Events.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models
{
    public class MainModel : Model
    {
        public EUser CreatedBy { get; set; }
        public long? CreatedById { get; set; }
        public EUser LastUpdateBy { get; set; }
        public long? LastUpdateById { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<ChangeLog> Changes { get; set; } = new List<ChangeLog>();
    }
}
