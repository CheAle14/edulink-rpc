﻿using System;
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
    public class EdulinkObject
    {
        public Edulink Client { get; set; }

        internal EdulinkObject(Edulink client)
        {
            Client = client;
        }
    }

    public class EdulinkObject<T> : EdulinkObject
    {
        public T Id { get; set; }
        protected EdulinkObject(Edulink client) : base(client)
        { 
        }
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
                    throw new EdulinkException(this.client, result.Value<string>("error"));
                }
            }
            throw new EdulinkException(this.client, resp.Content.ReadAsStringAsync().Result);
        }

        [Obsolete]
        internal JToken Call(string method) {
            return CallInternal(method, null);
        }

        internal JToken Call(string method, Dictionary<string, object> paramaters)
        {
            JToken retn = null;
            try
            {
                retn = CallInternal(method, paramaters);
            }
            catch(EdulinkException ex)
            {
                if (ex.Message.Contains("not logged in"))
                {
                    throw new EdulinkNotLoggedInException(this.client, new JsonRpcEventArgs(method, this.client.username), "", ex);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retn;
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

        public int MaxLoginRetryAttempts { get; set; } = 3;

        public event EventHandler<JsonRpcEventArgs> SendingRequest;

        internal void RaiseEvent(JsonRpcEventArgs e)
        {
            SendingRequest?.Invoke(this, e);
        }

        internal string _token;
        internal string AuthToken { get
            {
                if (string.IsNullOrWhiteSpace(_token))
                    login();
                return _token;
            } set { _token = value; } }
        internal int Establishment;
        internal int learnerId;

        public int LearnerId => learnerId;

        public string Username => username;

        internal string username;
        internal string password;

        internal Dictionary<int, AchievementType> types = new Dictionary<int, AchievementType>();
        internal AchievementType GetAchievementType(int id)
        {
            if(types.Count == 0)
            {
                var jtoken = interMediate("EduLink.AchievementBehaviourLookups", new Dictionary<string, object>()
                {
                    {"authtoken", AuthToken }
                });
                var achievements = jtoken["achievement_types"];
                var parsed = achievements.ToObject<API.AchievementType[]>();
                foreach(var model in parsed)
                {
                    if (types.ContainsKey(model.id))
                        continue;
                    var entity = AchievementType.Create(this, model);
                    types.Add(entity.Id, entity);
                }
            }
            return types[id];
        }
        internal AchievementType GetAchievementType(API.AchievementTypeIds id) => GetAchievementType((int)id);

        internal JToken interMediate(string method, Dictionary<string, object> param, int retryAttempts = 0)
        {
            JToken response = null;
            try
            {
                if(string.IsNullOrWhiteSpace(_token))
                { // havnt even connected, so we need to login first.
                    login();
                }
                response = Client.Call(method, param);
            }
            catch (EdulinkNotLoggedInException)
            {
                if(retryAttempts > MaxLoginRetryAttempts)
                {
                    // we have attempted too many times, so we quit.
                    throw new EdulinkSendException(this, new JsonRpcEventArgs(method, this.username), "Exceed maximum login retry attempts");
                }
                // we attempted, but the token has expired.
                // so we attempt to login, then re-try.
                login(); // login again, to get a new token.
                response = interMediate(method, param, retryAttempts + 1);
            }
            return response;
        }

        internal JToken login()
        {
            // this should NOT use intermediate, as it would get in an endless loop.
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
            learnerId = response["user"].Value<int>("id");
            return response;
        }

        internal JToken status()
        {
            var response = interMediate("EduLink.Status", new Dictionary<string, object>()
            {
                {"last_visible", 0 },
                {"authtoken", AuthToken }
            });
            learnerId = response["user"].Value<int>("id");
            return response;
        }

        internal JToken homework()
        {
            var response = interMediate("EduLink.Homework", new Dictionary<string, object>()
                {
                    {"authtoken", AuthToken }
                });
            return response;
        }

        internal JToken homeworkCompleted(int hwkId)
        {
            var response = interMediate("EduLink.HomeworkCompleted", new Dictionary<string, object>()
            {
                {"authtoken", AuthToken },
                {"homework_id", hwkId },
                {"learner_id", learnerId.ToString() },
                {"source", "EduLink" },
                {"completed", "true" }
            });
            return response;
        }

        internal JToken achievements()
        {
            var response = interMediate("EduLink.Achievement", new Dictionary<string, object>()
            {
                {"authtoken", AuthToken },
                {"learner_id", learnerId.ToString() }
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

        public Edulink(string uname, int learner_id, string token, int establishment_id = 60) : this(uname, null, establishment_id)
        {
            learnerId = learner_id;
            _token = token;
        }

        public JToken Login()
        {
            return login();
        }

        public JToken Status()
        {
            return status();
        }

        JToken timetable()
        {
            var response = interMediate("EduLink.Timetable", new Dictionary<string, object>()
            {
                {"authtoken", AuthToken },
                {"learner_id", learnerId.ToString() },
                {"date", DateTime.Now.ToString("yyyy-MM-dd") }
            });
            return response;
        }

        public Timetable GetTimetable()
        {
            var response = timetable();
            var weeks = response["weeks"];
            var weeksModel = weeks.ToObject<API.TableWeek[]>();
            var timeTableModel = new API.Timetable()
            {
                weeks = weeksModel.ToList()
            };
            return Timetable.Create(this, timeTableModel);
        }

        public Homework[] GetHomework(bool includePast = false)
        {
            var response = homework();
            var hmwk = response["homework"];
            API.Homework[] models = hmwk["current"].ToObject<API.Homework[]>();
            if(includePast)
            {
                API.Homework[] pastModels = hmwk["past"].ToObject<API.Homework[]>();
                var things = models.ToList();
                things.AddRange(pastModels);
                models = things.ToArray();
            }
            List<Homework> homeworks = new List<Homework>();
            foreach(var hwk in models)
            {
                var created = Homework.Create(this, hwk);
                homeworks.Add(created);
            }
            return homeworks.ToArray();
        }

        public JToken CompleteHomework(IHomework hwk)
        {
            return homeworkCompleted(hwk.Id);
        }

        public JToken CompleteHomework(int hwkId)
        {
            return homeworkCompleted(hwkId);
        }

        public List<Achievement> GetAchievements()
        {
            var things = achievements();
            var parsed = things["achievement"].ToObject<API.Achievement[]>();
            var listed = new List<Achievement>();
            foreach(var model in parsed)
            {
                listed.Add(Achievement.Create(this, model));
            }
            return listed;
        }
    }
}
