using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.API
{
    internal interface ICurrentable
    {
        bool is_current { get; set; }
    }
}
