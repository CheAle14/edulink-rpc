using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Person : EdulinkObject<int>
    {
        public string Title { get; internal set; }
        public string Forename { get; internal set; }
        public string Surname { get; internal set; }
        
        internal Person (Edulink client, API.Person model) : base(client)
        {
            Update(model);
        }

        internal static Person Create(Edulink client, API.Person model)
        {
            return new Person(client, model);
        }

        void Update(API.Person model)
        {
            this.Id = model.id ?? 0;
            this.Title = model.title;
            this.Forename = model.forename;
            this.Surname = model.surname;
        }
    }
}
