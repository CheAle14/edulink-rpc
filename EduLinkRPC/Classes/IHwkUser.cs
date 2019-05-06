using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public interface IHwkUser
    {
        string Name { get; }
        string UserName { get; }
        string Password { get; }

        Edulink Client {get;}

        List<IHomework> Homework { get; }

    }
}
