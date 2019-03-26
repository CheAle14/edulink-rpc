using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace EduLinkRPC.Classes
{
    public class BaseClass
    {
        public virtual string Name { get; set; }
        public virtual string Subject { get; }
        public virtual List<string> SubjectAliases { get; set; }
        public virtual List<HwkUser> Users { get; set; } = new List<HwkUser>();

        public virtual bool SameSubject(Homework hwk)
        {
            var subject = hwk.Subject;
            subject = subject.Replace("Maths", "Mathematics");
            return subject == Subject || SubjectAliases.Contains(subject);
        }
    }
}
