using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class Homework
    {
        internal int id { get; set; }
        internal string activity { get; set; }
        internal List<Attachment> attachments { get; set; }
        internal DateTime available_date { get; set; }
        internal string available_text { get; set; }
        internal bool completed { get; set; }
        internal string description { get; set; }
        internal string due_text { get; set; }
        internal int owner_id { get; set; }
        internal string set_by { get; set; }
        internal string status { get; set; }
        internal string subject { get; set; }
        internal string user_type { get; set; }
    }
}
