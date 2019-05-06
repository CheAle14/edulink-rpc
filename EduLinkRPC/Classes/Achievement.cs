using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class Achievement : EdulinkObject<int>
    {
        protected Achievement(Edulink client) : base(client)
        {
        }
        internal Achievement(Edulink client, API.Achievement model) : base(client)
        {
            Update(model);
        }

        public List<AchievementType> Types { get; set; } = new List<AchievementType>();
        public DateTime Date { get; set; }
        public int RecordedById { get; set; }
        public string Comments { get; set; }
        public int Points { get; set; }
        public string Lesson { get; set; }
        public bool Live { get; set; }

        internal void Update(API.Achievement model)
        {
            this.Id = int.Parse(model.id);
            this.Date = model.date;
            this.Types = model.type_ids.Select(x => this.Client.GetAchievementType(x)).ToList();
            this.RecordedById = int.Parse(model.recorded.employee_id);
            this.Comments = model.comments;
            this.Points = model.points;
            this.Lesson = model.lesson_information;
            this.Live = model.live;
        }

        internal static Achievement Create(Edulink client, API.Achievement model)
        {
            return new Achievement(client, model);
        }
    }
}
