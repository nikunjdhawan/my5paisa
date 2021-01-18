using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using Newtonsoft.Json;
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


    public static class MarketFeedManager
    {
        static MarketFeedRequest request = new MarketFeedRequest();
        static WebsocketClient client;
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private static string LoginCheck()
        {
            string cookie = "";
            IRestResponse response;
            while (cookie.Length == 0)
            {
                SessionManager.Instance.AddMessage("Calling LoginCheck() .... ");
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
        static MarketFeedManager()
        {
            request.ClientCode = "54965884";
            request.Method = "MarketFeedV3";
            request.Operation = "Subscribe";

            request.MarketFeedData = new List<MarketFeedData>();
            var data = request.MarketFeedData;

            data.Add(new MarketFeedData { Exch = "N", ExchType = "C", ScripCode = 15083 });
            data.Add(new MarketFeedData { Exch = "N", ExchType = "C", ScripCode = 1330 });
        }

        public static void AddScript(int scriptCode)
        {
            request.MarketFeedData.Add(new MarketFeedData { Exch = "N", ExchType = "C", ScripCode = scriptCode });
            string msg = SimpleJson.SerializeObject(request);
            if(client!=null)
                client.Send(msg);
        }

        public static void Start()
        {
            ExitEvent.Reset();
            var url = new Uri("wss://openfeed.5paisa.com/Feeds/api/chat?Value1=zdw053xuljn0d5q4potp5djs");
            var factory = new Func<ClientWebSocket>(() => InitSocket());
            using (client = new WebsocketClient(url, factory))
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                    SessionManager.Instance.AddMessage($"Reconnection happened, type: {info.Type}"));

                client.MessageReceived.Subscribe(msg => MessageReceived($"{msg}"));

                client.Start();
                string msg = SimpleJson.SerializeObject(request);

                client.Send(msg);
                ExitEvent.WaitOne();
            }
            client = null;
            SessionManager.Instance.AddMessage("*************Closing Market Feed*****************");

        }

        private static void MessageReceived(string msg)
        {
            try
            {
                // SessionManager.Instance.AddMessage($"Message received: {msg}");
                MarketFeedResponse[] response = JsonConvert.DeserializeObject<MarketFeedResponse[]>(msg);
                for (int i = 0; i < StrategyManager.AllStrategies.Count; i++)
                {
                    for (int j = 0; j < StrategyManager.AllStrategies[i].Trades.Count; j++)
                    {
                        var tc = StrategyManager.AllStrategies[i].Trades[j];
                        if (tc.ScriptCode == response[0].Token)
                            tc.LTP = response[0].LastRate;
                        
                    }
                    
                }
            }
            catch (System.Exception ex)
            {
                SessionManager.Instance.AddMessage($"Exception: {ex.ToString()}");
            }

        }
        public static void Stop()
        {
            ExitEvent.Set();
        }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class MarketFeedResponse
    {
        public string Exch { get; set; }
        public string ExchType { get; set; }
        public int Token { get; set; }
        public double LastRate { get; set; }
        public int LastQty { get; set; }
        public int TotalQty { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double OpenRate { get; set; }
        public double PClose { get; set; }
        public double AvgRate { get; set; }
        public int Time { get; set; }
        public int BidQty { get; set; }
        public double BidRate { get; set; }
        public int OffQty { get; set; }
        public double OffRate { get; set; }
        public int TBidQ { get; set; }
        public int TOffQ { get; set; }
        public DateTime TickDt { get; set; }
    }



}