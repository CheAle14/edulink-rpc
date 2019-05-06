using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using EduLinkRPC.Classes;

namespace EduLinkRPC.Addons.Discord
{
    public class BaseClass
    {
        public virtual string Name { get; set; }
        public virtual string Subject { get; }
        public virtual List<string> SubjectAliases { get; set; }
        public virtual List<DiscordHwkUser> Users { get; set; } = new List<DiscordHwkUser>();

        public virtual bool SameSubject(ClassHomework hwk)
        {
            var subject = hwk.Subject;
            subject = subject.Replace("Maths", "Mathematics");
            return subject == Subject || SubjectAliases.Contains(subject);
        }
    }
}
