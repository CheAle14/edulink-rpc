using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using EduLinkRPC.Classes;

namespace EduLinkRPC.Addons.Discord
{
    public class DiscordHwkUser : IHwkUser
    {
        [JsonIgnore]
        public string Name => User.Nickname ?? User.Username;

        public SocketGuildUser User;

        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore] // if password is empty, then we should only ping them if they are in classes.
        public bool IsNotifyOnly => string.IsNullOrWhiteSpace(Password);

        [JsonIgnore]
        public Edulink Client { get; protected set; }

        [JsonIgnore]
        public List<IHomework> Homework { get; set; }

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

        public DiscordHwkUser(SocketGuildUser user, string username, string password)
        {
            User = user;
            UserName = username;
            if (UserName == "cheale14")
                NotifyInDebug = true;
            Password = password;
            Client = new Edulink(username, password);
            NotifyOnDays = new List<int>() { 0, 1, 3, 5, 7, 14 };
            NotifyForSelfHomework = true;
        }

        public bool ShouldNotify(ClassHomework hwk)
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

            if (OtherHomeworksMarkedAsDone.Contains(hwk.Id))
                return false;

            return true;
        }

        public List<string> Classes = new List<string>();

        /// <summary>
        /// Homeworks of other people that are marked as done.
        /// </summary>
        public List<int> OtherHomeworksMarkedAsDone = new List<int>();

        [JsonConstructor]
        private DiscordHwkUser(string username, string password, SocketGuildUser user, List<int> notifyOnDays)
        {
            User = user;
            if(notifyOnDays == null)
            {
                notifyOnDays = new List<int>() { 0, 1, 3, 5, 7, 14 };
            }
            NotifyOnDays = notifyOnDays;
            Client = new Edulink(username, password);
            UserName = username;
            Password = password;
        }
    }
}
