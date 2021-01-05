using RestSharp;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace My5Paisa.Models
{
    public static class WebSessionManager

    {
        private static HttpClientHandler handler;
        private static HttpClient httpClient;
        static WebSessionManager()
        {
            Login();
        }
        public static void Login()
        {
            handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();
            httpClient = new HttpClient(handler);

            for (int i = 0; i < 10; i++)
            {
                ApiLogin();
                string resps = httpClient.GetStringAsync("https://www.5paisa.com/").Result;
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://www.5paisa.com/home/checkclient?Email=9999901554"),
                    Method = HttpMethod.Post,
                };
                var r = httpClient.Send(request);
                request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://www.5paisa.com/Home/Login?login.UserName=9999901554&login.ClientCode=54965884&login.Password=bptp%40987&login.DOB=01121980"),
                    Method = HttpMethod.Post,
                };

                r = httpClient.Send(request);
                resps = r.Content.ReadAsStringAsync().Result;
                if (resps.Contains("Invalid DOB"))
                {
                    continue;
                }
                else
                {
                    SessionManager.Instance.AddMessage("Login is Successfull !!");
                    resps = httpClient.GetStringAsync("https://trade.5paisa.com/trade/home").Result;
                    return;
                }

            }
            SessionManager.Instance.AddMessage("Login Failed 10 times");
        }


        public static void PlaceOrder(TradeCall tradeCall)
        {
            var doff = new DateTimeOffset(DateTime.Today);
            string today = doff.ToUnixTimeSeconds().ToString();
            string tomorrow = doff.AddDays(1).ToUnixTimeSeconds().ToString();
            string orderType = tradeCall.OrderType;
            string bracketstr = "RequestType=P&BuySell=" + tradeCall.OrderType + "&Symbol=&FullName=" + tradeCall.ScriptName + "&Name=" + tradeCall.ScriptName + "&Category=&Quantity=" + tradeCall.Qty + "&OldOrderNumber=&Exch=N&ExchType=C&Series=&DiscloseQty=0&CurrentPrice=" + tradeCall.Price + "&TriggerRate=0&IOC=false&ISSL=false&ScripCode=" + tradeCall.ScriptCode + "&TerminalId=&AfterHrs=false&SLStatus=false&isAtMarket=false&sProduct=&Validity=0&ValideDate=%2FDate(" + today + ")%2F&disableBuySell=false&CallFrom=&currStatus=&TradedQty=0&smotrailsl=&Volume=0&AdvanceBuy=false&BidRate=&OffRate=0&OrderValue=" + tradeCall.Price + "&ExchOrderID=&ExchOrderTime=%2FDate(" + today + ")%2F&AHPlaced=false&DelvIntra=&LastRate=0&LimitPriceforSL=0&TriggerPriceforSL=" + tradeCall.StopLossPrice + "&TrailingSL=0&LimitPriceforProfitOrder=" + tradeCall.TargetPrice + "&ISTMOOrder=Y&ISCoverOrder=N&TriggerPriceSLforCoverOrder=0&TrailingSLforCoverOrder=0&TriggerRateTMO=0&TrailingSLForNormalOrder=0&TickSize=0.05&SourceAPP=6&SliceEnable=N";
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://trade.5paisa.com/Trade/Orders/OrderProceed?" + bracketstr),
                Method = HttpMethod.Post,
            };
            var r = httpClient.Send(request);
            var strResponse = r.Content.ReadAsStringAsync().Result;
            SessionManager.Instance.AddMessage("*****************Start of Order***************************");
            SessionManager.Instance.AddMessage(bracketstr);
            SessionManager.Instance.AddMessage("------------------------Response---------------------------------");
            SessionManager.Instance.AddMessage(strResponse);
            SessionManager.Instance.AddMessage("*****************End of Order***************************");
        }

        private static void ApiLogin()
        {
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V3/LoginRequestMobileNewbyEmail");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs; JwtToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjU0OTY1ODg0IiwibmJmIjoxNjA5MzA1MjY4LCJleHAiOjE2MTcwODEyNjgsImlhdCI6MTYwOTMwNTI2OH0.OsDc5qZmsNE0-OThmsDi1t-uA_-KFzAAQDJEgtxi_IA");
            request.AddParameter("application/json", "{\n    \"head\": {\n        \"appName\": \"5P54965884\",\n        \"appVer\": \"1.0\",\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\n        \"osName\": \"WEB\",\n        \"requestCode\": \"5PLoginV3\",\n        \"userId\": \"m5rK5jEwGtK\",\n        \"password\": \"Vw0EUSzdh6P\"\n    },\n    \"body\": {\n        \"Email_id\": \"4qttteNalFNCUWTjg4RfewaI7Bz0xMTzyJ7mjuy11hU=\",\n        \"Password\": \"7FCqAhrPzqDxJb+0447o4g==\",\n        \"LocalIP\": \"182.69.74.25\",\n        \"PublicIP\": \"182.69.74.25\",\n        \"HDSerailNumber\": \"DEB9-4D82\",\n        \"MACAddress\": \"F4-30-B9-90-CD-B9\",\n        \"MachineID\": \"64-6E-69-7D-CC-4F\",\n        \"VersionNo\": \"1.7\",\n        \"RequestNo\": \"1\",\n        \"My2PIN\": \"Y7VejiwJaRwlIfZBKHaseA==\",\n        \"ConnectionType\": \"1\"\n    }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            LoginRoot loginRoot = JsonConvert.DeserializeObject(response.Content, typeof(LoginRoot)) as LoginRoot;
        }

    }
}