using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EduLinkRPC.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    internal class JsonRpcResponse
    {

    }


    internal class RPCClient
    {
        internal string url = "";
        internal Edulink client;
        JToken CallInternal(string method, Dictionary<string, object> params_)
        {
            var req = new JsonRpcRequest();
            req.id = "1"; // doesnt appear to change
            req.method = method;
            req.params_ = params_;
            req.uuid = Guid.NewGuid().ToString(); // not sure why/what this does

            var str = JsonConvert.SerializeObject(req);
            var buffer = System.Text.Encoding.UTF8.GetBytes(str);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = null;
            client.RaiseEvent(new JsonRpcEventArgs(method, client.username));
            using (HttpClient httpclient = new HttpClient()) { resp = httpclient.PostAsync(url + method, byteContent).Result; }
            if(resp.IsSuccessStatusCode)
            { // should always return 200, even if it errors...
                var jsonResponse = resp.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonConvert.DeserializeObject(jsonResponse) as JObject;
                var result = jsonObject["result"];
                if (result.Value<bool>("success"))
                {
                    return result;
                } else
                {
                    throw new Exception("Failed: " + result.Value<string>("error"));
                }
            }
            throw new Exception("Failed: " + resp.Content.ReadAsStringAsync().Result);
        }

        internal JToken Call(string method) {
            return CallInternal(method, null);
        }
        internal JToken Call(string method, Dictionary<string, object> paramaters)
        {
            return CallInternal(method, paramaters);
        }
        public RPCClient(Edulink _client, string _url)
        {
            client = _client;
            url = _url;
        }
    }

    public class JsonRpcEventArgs : EventArgs
    {
        public readonly string Method;
        public readonly string Username;

        internal JsonRpcEventArgs(string method, string uname)
        {
            Method = method;
            Username = uname;
        }
    }


    public class Edulink
    {
        public static string Url = "https://www.edulinkone.com/api/?method=";
        internal RPCClient Client;

        public event EventHandler<JsonRpcEventArgs> SendingRequest;

        internal void RaiseEvent(JsonRpcEventArgs e)
        {
            SendingRequest?.Invoke(this, e);
        }

        internal string _token;
        internal string AuthToken { get
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(_token))
                        throw new Exception("not logged in"); // no point asking, since we arent even logged in to begin with
                    var statusResponse = status();
                } catch (Exception ex)
                {
                    if(ex.Message.Contains("not logged in"))
                    {
                        login(); // try to login
                    }
                }
                return _token;
            } set { _token = value; } }
        internal int Establishment;
        internal int LearnerId;

        internal string username;
        internal string password;

        internal JToken login()
        {
            var response = Client.Call("EduLink.Login", new Dictionary<string, object>()
            {
                {"establishment_id", Establishment },
                {"username", username },
                {"password", password },
                {"from_app", false },
                {"ui_info", new Dictionary<string, object> () {
                    {"format", 2 },
                    {"git_sha", "c29864442002e2c0cb7ee2adec87bb4f75482a64" },
                    {"version", "0.3.235" }
                } }
            });
            AuthToken = response.Value<string>("authtoken");
            return response;
        }

        internal JToken status()
        {
            var response = Client.Call("EduLink.Status", new Dictionary<string, object>()
            {
                {"last_visible", 0 },
                {"authtoken", _token }
            });
            LearnerId = response["user"].Value<int>("id");
            return response;
        }

        internal JToken homework()
        {
            var response = Client.Call("EduLink.Homework", new Dictionary<string, object>()
                {
                    {"authtoken", AuthToken }
                });
            return response;
        }

        internal JToken homeworkCompleted(int hwkId)
        {
            var response = Client.Call("EduLink.HomeworkCompleted", new Dictionary<string, object>()
            {
                {"authtoken", AuthToken },
                {"homework_id", hwkId },
                {"learner_id", LearnerId },
                {"source", "CheAle14-App" }
            });
            return response;
        }

        public Edulink(string uname, string pwd, int establishment_id = 60)
        {
            username = uname;
            password = pwd;
            Establishment = establishment_id;
            Client = new RPCClient(this, Url);
        }

        public JToken Login()
        {
            return login();
        }

        public JToken Status()
        {
            return status();
        }

        public JToken GetHomework()
        {
            return homework();
        }

        public JToken CompleteHomework(Homework hwk)
        {
            return homeworkCompleted(hwk.Id);
        }

        public JToken CompleteHomework(int hwkId)
        {
            return homeworkCompleted(hwkId);
        }
    }
}
