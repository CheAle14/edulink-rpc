using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class TableWeek : ICurrentable
    {
        public string name { get; set; }
        public List<TableDay> days { get; set; }
        public bool is_current { get; set; }
    }
}
