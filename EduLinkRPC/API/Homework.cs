using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Homework
    {
        public int id { get; set; }
        public string activity { get; set; }
        public List<Attachment> attachments { get; set; }
        public DateTime available_date { get; set; }
        public string available_text { get; set; }
        public bool completed { get; set; }
        public string description { get; set; }
        public DateTime due_date { get; set; }
        public string due_text { get; set; }
        public int owner_id { get; set; }
        public string set_by { get; set; }
        public string status { get; set; }
        public string subject { get; set; }
        public string user_type { get; set; }
    }
}
