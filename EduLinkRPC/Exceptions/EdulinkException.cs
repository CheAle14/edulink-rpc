using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC
{
    public class EdulinkException : Exception
    {
        public Edulink Client { get; internal set; }
        internal EdulinkException(Edulink client, string message) : base(message)
        {
            Client = client;
        }
        internal EdulinkException(Edulink client, string message, Exception innerException) : base(message, innerException)
        {
            Client = client;
        }
        public override string ToString()
        {
            var ss = base.ToString();
            if (Client != null && !string.IsNullOrWhiteSpace(Client.username))
                ss = $"{Client.username} :: {ss}";
            return ss;
        }
    }

    public class EdulinkSendException : EdulinkException
    {
        public JsonRpcEventArgs Request { get; set; }
        internal EdulinkSendException(Edulink client, JsonRpcEventArgs request, string message) : base(client, message)
        {
            Request = request;
        }

        internal EdulinkSendException(Edulink client, JsonRpcEventArgs request, string message, Exception innerException) : base(client, message, innerException)
        {
            Request = request;
        }
    }

    public class EdulinkNotLoggedInException : EdulinkSendException
    {
        internal EdulinkNotLoggedInException(Edulink client, JsonRpcEventArgs request) : base(client, request, "")
        {
        }

        internal EdulinkNotLoggedInException(Edulink client, JsonRpcEventArgs request, string message, Exception innerException) : base(client, request, message, innerException)
        {
        }
    }
}
