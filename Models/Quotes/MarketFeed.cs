using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using RestSharp;
using Websocket.Client;

namespace My5Paisa.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class MarketFeedData
    {
        public string Exch { get; set; }
        public string ExchType { get; set; }
        public int ScripCode { get; set; }
    }

    public class MarketFeedRequest
    {
        public string Method { get; set; }
        public string Operation { get; set; }
        public string ClientCode { get; set; }
        public List<MarketFeedData> MarketFeedData { get; set; }
    }


    public static class MarketFeed
    {
        static MarketFeedRequest request = new MarketFeedRequest();
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private static string LoginCheck()
        {
            string cookie = "";
            IRestResponse response;
            while (cookie.Length == 0)
            {
                var client = new RestClient("https://openfeed.5paisa.com/Feeds/api/UserActivity/LoginCheck");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "");
                request.AddParameter("application/json", "{\r\n\"head\": {\r\n    \"appName\": \"5P54965884\",\r\n    \"appVer\": \"1.0\",\r\n    \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\r\n    \"osName\": \"Web\",\r\n    \"requestCode\":\"5PLoginCheck\"\r\n},\r\n\"body\": {\r\n    \"LoginId\" : \"54965884\",\r\n    \"RegistrationID\": \"zdw053xuljn0d5q4potp5djs\"\r\n}\r\n}", ParameterType.RequestBody);
                response = client.Execute(request);
                if (response.Cookies.Count > 0)
                {
                    cookie = response.Cookies[0].Value;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            return cookie;
        }
        private static ClientWebSocket InitSocket()
        {
            var cookie = LoginCheck();
            ClientWebSocket socket = new ClientWebSocket
            {
                Options =
                    {
                        Cookies = new System.Net.CookieContainer()
                    }

            };
            socket.Options.Cookies.Add(new System.Net.Cookie(".ASPXAUTH", cookie, "/", "openfeed.5paisa.com"));
            return socket;
        }
        static MarketFeed()
        {
            request.ClientCode = "54965884";
            request.Method = "MarketFeedV3";
            request.Operation = "Subscribe";

            request.MarketFeedData = new List<MarketFeedData>();
            var data = request.MarketFeedData;

            data.Add(new MarketFeedData { Exch = "N", ExchType = "C", ScripCode = 15083 });



        }

        public static void Start()
        {
            var url = new Uri("wss://openfeed.5paisa.com/Feeds/api/chat?Value1=zdw053xuljn0d5q4potp5djs");
            var factory = new Func<ClientWebSocket>(() => InitSocket());
            using (var client = new WebsocketClient(url, factory))
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                    SessionManager.Instance.AddMessage($"Reconnection happened, type: {info.Type}"));

                client.MessageReceived.Subscribe(msg => SessionManager.Instance.AddMessage($"Message received: {msg}"));

                client.Start();
                string msg = SimpleJson.SerializeObject(request);

                //string msg = "{\"Method\":\"MarketFeedV3\",\"Operation\":\"Subscribe\",\"ClientCode\":\"54965884\",\"MarketFeedData\":[{\"Exch\":\"N\",\"ExchType\":\"C\",\"ScripCode\":15083},{\"Exch\":\"N\",\"ExchType\":\"C\",\"ScripCode\":14732}]}";
                client.Send(msg);
                ExitEvent.WaitOne();
            }

        }
        public static void Stop()
        {
            ExitEvent.Set();

        }

    }
}