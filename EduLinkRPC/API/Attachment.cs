using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal class Attachment
    {
        internal string filename { get; set; }
        internal long filesize { get; set; }
        internal string mime_type { get; set; }
    }
}
