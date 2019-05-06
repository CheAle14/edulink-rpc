using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    public class Attachment
    {
        public string filename { get; set; }
        public long filesize { get; set; }
        public string mime_type { get; set; }
    }
}
