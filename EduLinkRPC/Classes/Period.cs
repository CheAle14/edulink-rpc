using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Period : EdulinkObject<int>
    {
        public bool Empty { get; internal set; }
        public string Name { get; internal set; }
        public int SimsId { get; internal set; }

        internal Period(Edulink client, API.Period model) : base(client)
        {
            Update(model);
        }

        internal static Period Create(Edulink client, API.Period model)
        {
            return new Period(client, model);
        }

        void Update(API.Period model)
        {
            this.Id = model.id;
            this.Empty = model.empty;
            this.Name = model.name;
            this.SimsId = model.sims_id;
        }
    }
}
