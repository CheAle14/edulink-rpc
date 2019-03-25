using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Homework
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }

        [JsonProperty("activity")]
        public string Activity { get; internal set; }

        [JsonProperty("attachments")]
        public List<HwkAttachment> Attachments { get; internal set; } = new List<HwkAttachment>();

        [JsonProperty("available_date")]
        public DateTime AvailableDate { get; internal set; }

        [JsonProperty("available_text")]
        public string AvailableText { get; internal set; }

        public int DaysAvailable { get
            {
                return int.Parse(AvailableText.Replace("days ago", ""));
            } }

        [JsonProperty("completed")]
        public bool Completed { get; internal set; }

        [JsonProperty("description")]
        public string Description { get; internal set; }

        [JsonProperty("due_text")]
        public string DueText { get; internal set; }

        [JsonProperty("due_date")]
        public DateTime DueDate { get; internal set; }

        public int DaysUntilDue { get
            {
                return (int)((DueDate - DateTime.Now).TotalDays);
            } }

        [JsonProperty("owner_id")]
        public int OwnerId { get; internal set; }

        [JsonProperty("set_by")]
        public string SetBy { get; internal set; }

        [JsonProperty("status")]
        public string Status { get; internal set; }

        [JsonProperty("subject")]
        public string Subject { get; internal set; }

        /// <summary>
        /// Type of the user who set the homework, "learner" or "teacher"
        /// </summary>
        [JsonProperty("user_type")]
        public string UserType { get; internal set; }

        public List<BaseClass> GivenTo { get; set; } = new List<BaseClass>();

        public List<HwkUser> AppliesTo { get; set; } = new List<HwkUser>();

        /// <summary>
        /// All users directly, and indirectly via classes, who recieved the homework
        /// </summary>
        public List<HwkUser> TotalUsersApplied { get
            {
                var list = new List<HwkUser>();
                list.AddRange(AppliesTo);
                foreach(var cls in GivenTo)
                {
                    foreach(var u in cls.Users)
                    {
                        if (!list.Contains(u))
                            list.Add(u);
                    }
                }
                return list;
            } }
    }
}
