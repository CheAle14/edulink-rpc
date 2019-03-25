using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EduLinkRPC.Classes
{
    public class HwkUser
    {
        [JsonIgnore]
        public string Name => User.Nickname ?? User.Username;
        public SocketGuildUser User;

        public string Username;
        public string Password;

        [JsonIgnore]
        public Edulink EdulinkClient;

        // homework
        // preferences
        // classes...
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Homework> Homework;

        /// <summary>
        /// Days that we notify on, with 0 being a notification for bringing it in on the day.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<int> NotifyOnDays;

        /// <summary>
        /// Notify for homeworks given by students, rather than teachers.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [System.ComponentModel.DefaultValue(true)]
        public bool NotifyForSelfHomework;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [System.ComponentModel.DefaultValue(false)]
        public bool NotifyInDebug;

        public HwkUser(SocketGuildUser user, string username, string password)
        {
            User = user;
            Username = username;
            if (Username == "cheale14")
                NotifyInDebug = true;
            Password = password;
            EdulinkClient = new Edulink(username, password);
            NotifyOnDays = new List<int>() { 0, 1, 3, 5, 7, 14 };
            NotifyForSelfHomework = true;
        }

        public bool ShouldNotify(Homework hwk)
        {
            // we assume that this hwk is passed because it 'AppliesTo' us

            int date = hwk.DaysUntilDue;
            if (!NotifyOnDays.Contains(date))
                return false;
            // check if self homework

            if (hwk.UserType == "learner" && !NotifyForSelfHomework)
                return false;

            // debug check
#if DEBUG
            if (!NotifyInDebug)
                return false;
#endif
            if (hwk.Completed)
                return false;
            return true;
        }

        public List<string> Classes = new List<string>();

        [JsonConstructor]
        private HwkUser(string username, string password, SocketGuildUser user, List<int> notifyOnDays)
        {
            User = user;
            if(notifyOnDays == null)
            {
                notifyOnDays = new List<int>() { 0, 1, 3, 5, 7, 14 };
            }
            NotifyOnDays = notifyOnDays;
            EdulinkClient = new Edulink(username, password);
            Username = username;
            Password = password;
        }
    }
}
