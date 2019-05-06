using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class AchievementType : EdulinkObject<int>
    {
        [Obsolete("", true)]
        protected AchievementType(Edulink client) : base(client)
        {
        }
        internal AchievementType(Edulink client, API.AchievementType model) : base(client)
        {
            Update(model);
        }

        public bool Active { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }

        internal static AchievementType Create(Edulink client, API.AchievementType model)
        {
            return new AchievementType(client, model);
        }

        internal void Update(API.AchievementType model)
        {
            this.Id = model.id;
            this.Active = model.active;
            this.Code = model.code;
            this.Description = model.description;
            this.Position = model.position;
            this.Points = model.points;
        }
    }
}
