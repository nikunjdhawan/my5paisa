using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Hangfire;
using System;
using System.Linq;

namespace My5Paisa.Models
{
    public class SessionManager

    {
        private List<string> messages = null;
        public List<string> Messages
        {
            get { return messages; }
        }

        private static SessionManager instance = null;
        public static SessionManager Instance
        {
            get
            {
                lock (typeof(SessionManager))
                {
                    if (instance != null) return instance;
                    instance = new SessionManager();
                    return instance;
                }
            }
        }

        private NetPositionRoot netPositions;
        public NetPositionRoot NetPositions
        {
            get { return netPositions; }
        }

        private OrderBookRoot orders;
        public OrderBookRoot Orders
        {
            get { 
                
                if(orders == null) GetOrderBook();
                return orders; }
        }

        private List<TradeCall> trades = new List<TradeCall>();
        public List<TradeCall> Trades
        {
            get { return trades; }
        }


        private double margin;
        public double Margin
        {
            get { return margin; }
        }


        public void AddMessage(string msg)
        {
            messages.Add(msg);
        }
        private SessionManager()
        {
            messages = new List<string>();
            Login();
            GetMargin();
            GetNetPositions();

        }
        private void Login()
        {
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V3/LoginRequestMobileNewbyEmail");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs; JwtToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjU0OTY1ODg0IiwibmJmIjoxNjA5MzA1MjY4LCJleHAiOjE2MTcwODEyNjgsImlhdCI6MTYwOTMwNTI2OH0.OsDc5qZmsNE0-OThmsDi1t-uA_-KFzAAQDJEgtxi_IA");
            request.AddParameter("application/json", "{\n    \"head\": {\n        \"appName\": \"5P54965884\",\n        \"appVer\": \"1.0\",\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\n        \"osName\": \"WEB\",\n        \"requestCode\": \"5PLoginV3\",\n        \"userId\": \"m5rK5jEwGtK\",\n        \"password\": \"Vw0EUSzdh6P\"\n    },\n    \"body\": {\n        \"Email_id\": \"4qttteNalFNCUWTjg4RfewaI7Bz0xMTzyJ7mjuy11hU=\",\n        \"Password\": \"7FCqAhrPzqDxJb+0447o4g==\",\n        \"LocalIP\": \"182.69.74.25\",\n        \"PublicIP\": \"182.69.74.25\",\n        \"HDSerailNumber\": \"DEB9-4D82\",\n        \"MACAddress\": \"F4-30-B9-90-CD-B9\",\n        \"MachineID\": \"64-6E-69-7D-CC-4F\",\n        \"VersionNo\": \"1.7\",\n        \"RequestNo\": \"1\",\n        \"My2PIN\": \"Y7VejiwJaRwlIfZBKHaseA==\",\n        \"ConnectionType\": \"1\"\n    }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            LoginRoot loginRoot = JsonConvert.DeserializeObject(response.Content, typeof(LoginRoot)) as LoginRoot;

            Messages.Add(loginRoot.body.EmailId);
        }

        private void GetMargin()
        {
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V3/Margin");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs; JwtToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjU0OTY1ODg0IiwibmJmIjoxNjA5MzA1MjY4LCJleHAiOjE2MTcwODEyNjgsImlhdCI6MTYwOTMwNTI2OH0.OsDc5qZmsNE0-OThmsDi1t-uA_-KFzAAQDJEgtxi_IA");
            request.AddParameter("application/json", "{\n    \"head\": {\n        \"appName\": \"5P54965884\",\n        \"appVer\": \"1.0\",\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\n        \"osName\": \"WEB\",\n        \"requestCode\": \"5PMarginV3\",\n        \"userId\": \"m5rK5jEwGtK\",\n        \"password\": \"Vw0EUSzdh6P\"\n    },\n    \"body\": {\n        \"ClientCode\": \"54965884\"\n    }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            MarginRoot root = JsonConvert.DeserializeObject(response.Content, typeof(MarginRoot)) as MarginRoot;
            Messages.Add(root.body.EquityMargin[0].AvailableMargin.ToString());
            margin = root.body.EquityMargin[0].AvailableMargin;


        }

        public NetPositionRoot GetNetPositions()
        {
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V1/NetPositionNetWise");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs; JwtToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjU0OTY1ODg0IiwibmJmIjoxNjA5NDAyMTg0LCJleHAiOjE2MTcxNzgxODQsImlhdCI6MTYwOTQwMjE4NH0.cKnFAQZw2LupT4hKUyPMLlKiTtkMaeGabLvvjKWn2-Q");
            request.AddParameter("application/json", "{\n    \"head\": {\n        \"appName\": \"5P54965884\",\n        \"appVer\": \"1.0\",\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\n        \"osName\": \"WEB\",\n        \"requestCode\": \"5PNPNWV1\",\n        \"userId\": \"m5rK5jEwGtK\",\n        \"password\": \"Vw0EUSzdh6P\"\n    },\n    \"body\": {\n        \"ClientCode\": \"54965884\"\n    }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            NetPositionRoot root = JsonConvert.DeserializeObject(response.Content, typeof(NetPositionRoot)) as NetPositionRoot;

            if (root != null && root.body.NetPositionDetail.Count > 0)
                Messages.Add(DateTime.Now.TimeOfDay + ": " + root.body.NetPositionDetail[0].MTOM.ToString());
            else
                Messages.Add(DateTime.Now.TimeOfDay + ": " + "No Net positions");
            netPositions = root;
            return root;
        }


        public OrderBookRoot GetOrderBook()
        {
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V2/OrderBook");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs");
            request.AddParameter("application/json", "{\n    \"head\": {\n        \"appName\": \"5P54965884\",\n        \"appVer\": \"1.0\",\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\n        \"osName\": \"WEB\",\n        \"requestCode\": \"5POrdBkV2\",\n        \"userId\": \"m5rK5jEwGtK\",\n        \"password\": \"Vw0EUSzdh6P\"\n    },\n    \"body\": {\n        \"ClientCode\": \"54965884\"\n    }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            orders = JsonConvert.DeserializeObject(response.Content, typeof(OrderBookRoot)) as OrderBookRoot;
            return orders;
        }



        public void ScanScripts()
        {
            if(SessionManager.Instance.Trades.Count>0) return;
            var client = new RestClient("https://www1.nseindia.com/live_market/dynaContent/live_analysis/pre_open/nifty.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Accept-Language", "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);
            Nifty50Root nifty50 = JsonConvert.DeserializeObject(response.Content, typeof(Nifty50Root)) as Nifty50Root;

            int buyOrdersCount = (int)(nifty50.advances / (nifty50.advances + nifty50.declines) * 10);

            foreach (var item in nifty50.data.Where(i => i.perChn > 0 && i.iep < 5000).OrderBy(i => i.perChn).Take(buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Buy"};
                Buy(tc);
            }
            System.Console.WriteLine("==============");
            foreach (var item in nifty50.data.Where(i => i.perChn < 0 && i.iep < 5000).OrderByDescending(i => i.perChn).Take(10 - buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Sell"};
                Sell(tc);
            }
        }

        private static void Buy(TradeCall tc)
        {
            SessionManager.Instance.Trades.Add(tc);
            SessionManager.Instance.AddMessage("Buy: " + tc.ScriptName + " at " + tc.Price.ToString("f") + " Take Profit at: " + tc.TargetPrice.ToString("c") + " Stop Loss at: " + tc.StopLossPrice.ToString("c"));
        }

        private static void Sell(TradeCall tc)
        {
            SessionManager.Instance.Trades.Add(tc);
            SessionManager.Instance.AddMessage("Buy: " + tc.ScriptName + " at " + tc.Price.ToString("f") + " Take Profit at: " + tc.TargetPrice.ToString("c") + " Stop Loss at: " + tc.StopLossPrice.ToString("c"));
        }

    }
}