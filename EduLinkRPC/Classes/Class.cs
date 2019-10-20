using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Class : EdulinkObject<int>
    {
        protected Class(Edulink client) : base(client)
        {
        }

        public string Name { get; internal set; }
        public string Subject { get; internal set; }

        internal Class(Edulink client, API.Class model) : this(client)
        {
            Update(model);
        }

        internal static Class Create(Edulink client, API.Class model)
        {
            return new Class(client, model);
        }

        internal void Update(API.Class model)
        {
            this.Id = model.id;
            this.Name = model.name;
            this.Subject = model.subject;
        }
    }
}
