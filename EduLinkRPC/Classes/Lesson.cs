using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EduLinkRPC.Classes
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Lesson : EdulinkObject
    {
        private string DebuggerDisplay {  get
            {
                return $"{PeriodId}: {Room.Name} {Teachers} {Class.Name} {Class.Subject}";
            } }
        public int PeriodId { get; internal set; }
        public Room Room { get; internal set; }
        public Person Teacher { get; internal set; }
        /// <summary>
        /// A string representation of <see cref="Teacher"/>
        /// </summary>
        public string Teachers { get; internal set; }
        public Class Class { get; internal set; }

        internal Lesson(Edulink client, API.Lesson model) : base(client)
        {
            Update(model);
        }

        internal static Lesson Create(Edulink client, API.Lesson model)
        {
            return new Lesson(client, model);
        }

        void Update(API.Lesson model)
        {
            this.PeriodId = model.period_id;
            this.Room = Room.Create(this.Client, model.room);
            this.Teacher = model.teacher.id.HasValue ? Person.Create(this.Client, model.teacher) : null;
            this.Teachers = model.teachers;
            this.Class = Class.Create(this.Client, model.teaching_group);
        }
    }
}
