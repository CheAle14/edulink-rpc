using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class Lesson
    {
        public int period_id { get; set; }
        public Room room { get; set; }
        public Person teacher { get; set; }
        public string teachers { get; set; }
        public Class teaching_group { get; set; }
    }
}
