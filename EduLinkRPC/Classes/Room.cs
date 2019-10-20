using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Room : EdulinkObject<int>
    {
        public string Name { get; internal set; }

        internal Room(Edulink client, API.Room model) : base(client)
        {
            Update(model);
        }

        internal static Room Create(Edulink client, API.Room model)
        {
            return new Room(client, model);
        }

        void Update(API.Room model)
        {
            this.Id = model.id ?? -1;
            this.Name = model.name;
        }
    }
}
