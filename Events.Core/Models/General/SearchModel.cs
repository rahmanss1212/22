using System;
using Events.Service.Service.DataServices;

namespace Events.Core.Models.General
{
    public class SearchModel
    {
        public string key { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string status { get; set; }
        public string sectors { get; set; }
        public string orgs { get; set; }
        public string apts { get; set; }
        public string employees { get; set; }

        public string departments { get; set; }
        public PaggingModel Page { get; set; }
    }
}