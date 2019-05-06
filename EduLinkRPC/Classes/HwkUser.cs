using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace EduLinkRPC.Classes
{
    public class HwkUser : IHwkUser
    {
        public string Name { get; set; }

        public string UserName { get; }

        public string Password { get; }

        public Edulink Client { get; set; }

        public List<IHomework> Homework { get; protected set; }

        public HwkUser(string name, string uName, string password, int establishment = 60)
        {
            Name = name;
            UserName = uName;
            Password = password;
            Client = new Edulink(uName, password, establishment);
        }

        public void RefreshHomework()
        {
            var hwk = Client.GetHomework();
            Homework = hwk.Select(x => x as IHomework).ToList();
        }
    }
}
