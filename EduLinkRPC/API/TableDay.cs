using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class TableDay : ICurrentable
    {
        public int cycle_day_id { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public bool is_current { get; set; }
        public List<Lesson> lessons { get; set; }
        public List<Period> periods { get; set; }
    }
}
