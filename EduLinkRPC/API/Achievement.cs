using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class Achievement 
    {
        public string id { get; set; }
        public AchievementTypeIds[] type_ids { get; set; }
        public object activity_id { get; set; }
        public DateTime date { get; set; }
        public Recorded recorded { get; set; }
        public string comments { get; set; }
        public int points { get; set; }
        public string lesson_information { get; set; }
        public bool live { get; set; }
    }
}
