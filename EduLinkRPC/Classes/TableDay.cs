using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EduLinkRPC.Classes
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TableDay : EdulinkObject<int>
    {
        private string DebuggerDisplay { get
            {
                return $"{Date.ToString("yyyy-MM-dd")} {Name} {Lessons.Count}";
            } }
        public DateTime Date { get; internal set; }
        public string Name { get; internal set; }
        public bool IsCurrent { get; internal set; }
        public List<Lesson> Lessons { get; internal set; }
        public List<Period> Periods { get; internal set; }

        internal TableDay(Edulink client, API.TableDay model) : base(client)
        {
            Update(model);
        }

        internal static TableDay Create(Edulink client, API.TableDay model)
        {
            return new TableDay(client, model);
        }

        void Update(API.TableDay model)
        {
            this.Id = model.cycle_day_id;
            this.Date = model.date;
            this.Name = model.name;
            this.IsCurrent = model.is_current;
            this.Lessons = model.lessons.Select(x => Lesson.Create(Client, x)).ToList();
            this.Periods = model.periods.Select(x => Period.Create(Client, x)).ToList();
        }
    }
}
