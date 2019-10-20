using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EduLinkRPC.Classes
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Timetable : EdulinkObject
    {
        private string DebuggerDisplay {  get
            {
                string tt = "[{";
                foreach(var week in Weeks)
                {
                    tt += $"{week.Name}: {week.IsCurrent}, {week.Days.Count}, ";
                }
                tt += "}]";
                return tt;
            } }

        public List<TableWeek> Weeks { get; set; }

        internal Timetable(Edulink client, API.Timetable model) : base(client)
        {
            Update(model);
        }
            
        internal static Timetable Create(Edulink client, API.Timetable model)
        {
            return new Timetable(client, model);
        }

        void Update(API.Timetable model)
        {
            Weeks = model.weeks.Select(x => TableWeek.Create(Client, x)).ToList();
        }
    }
}
