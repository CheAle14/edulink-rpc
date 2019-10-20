using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace EduLinkRPC.Classes
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TableWeek : EdulinkObject
    {
        private string DebuggerDisplay { get
            {
                return this.Name;
            } }
        public string Name { get; internal set; }
        public List<TableDay> Days { get; internal set; }
        public bool IsCurrent { get; internal set; }

        internal TableWeek(Edulink client, API.TableWeek model) : base(client)
        {
            Update(model);
        }
        internal static TableWeek Create(Edulink client, API.TableWeek model)
        {
            return new TableWeek(client, model);
        }
        void Update(API.TableWeek model)
        {
            this.Name = model.name;
            this.Days = model.days.Select(x => TableDay.Create(this.Client, x)).ToList();
            this.IsCurrent = model.is_current;
        }
    }
}
