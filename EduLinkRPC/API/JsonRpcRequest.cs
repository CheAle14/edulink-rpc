using System.Collections.Generic;
using Newtonsoft.Json;

namespace EduLinkRPC
{
    internal class JsonRpcRequest
    {
        public string jsonrpc = "2.0";
        public string method;
        [JsonProperty("params")]
        public Dictionary<string, object> params_;
        public string uuid;
        public string id;
    }
}
