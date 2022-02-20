using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Core.Models.DPE
{
    public class CountingOrgView
    {
        public long Id { get; set; }
        public String Orgname { get; set; }
        public int Total { get; set; }
        public int D_T { get; set; }
        public int N_T { get; set; }
        public int Critical { get; set; }
        public int D_C { get; set; }
        public int Medium { get; set; }
        public int D_M { get; set; }
        public int Low { get; set; }
        public int D_L { get; set; }
        public int Unkown { get; set; }
        public int D_U { get; set; }
    }
}
