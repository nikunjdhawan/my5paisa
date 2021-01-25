using RestSharp;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections;
using System.Collections.Specialized;

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


        public static bool PlaceOrder(TradeCall tradeCall)
        {
            string bracketstr = GetRequest(tradeCall);

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
            return strResponse.Contains("Xmitted");
        }

        public static void ModifyOrder(OrderBookDetail order, double stoploss=0, double target = 0)
        {
            var doff = new DateTimeOffset(DateTime.Today);
            string today = doff.ToUnixTimeSeconds().ToString();

            NameValueCollection ht = new NameValueCollection();
            ht["RequestType"] = "M";
            ht["BuySell"] = order.BuySell == "S" ? "Sell" : "Buy";
            ht["Symbol"] = "";
            ht["FullName"] = order.ScripName;
            ht["Name"] = order.ScripName;
            ht["Category"] = "";
            ht["Quantity"] = order.Qty.ToString();
            ht["OldOrderNumber"] = "";
            ht["Exch"] = "N";
            ht["ExchType"] = "C";
            ht["Series"] = "";
            ht["DiscloseQty"] = "0";
            if(target > 0)
            {
                ht["CurrentPrice"] = target.ToString();
                ht["TriggerRate"] = "0";
                ht["isAtMarket"] = "false";
                ht["OrderValue"] = (target * order.Qty).ToString();

            }
            if(stoploss>0)
            {
                ht["CurrentPrice"] = "0";
                ht["TriggerRate"] = stoploss.ToString();
                ht["isAtMarket"] = "true";
                ht["OrderValue"] = (stoploss * order.Qty).ToString();

            }
            ht["TriggerRateTMO"] = "0";
            
            ht["IOC"] = "false";
            ht["ISSL"] = "false";
            ht["ScripCode"] = order.ScripCode.ToString();
            ht["TerminalId"] = "";
            ht["AfterHrs"] = "false";
            ht["SLStatus"] = "false";
            
            ht["sProduct"] = "";
            ht["Validity"] = "0";
            ht["ValideDate"] = "%2FDate(" + today + ")%2F";
            ht["disableBuySell"] = "false";
            ht["CallFrom"] = "";
            ht["currStatus"] = "";
            ht["TradedQty"] = "0";
            ht["smotrailsl"] = "";
            ht["Volume"] = "0";
            ht["AdvanceBuy"] = "false";
            ht["BidRate"] = "";
            ht["OffRate"] = "";
            
            ht["ExchOrderID"] = order.ExchOrderID;
            ht["ExchOrderTime"] = order.ExchOrderTime;
            ht["AHPlaced"] = "false";
            ht["DelvIntra"] = "";
            ht["LastRate"] = "0";
            ht["LimitPriceforSL"] = "0";
            ht["TriggerPriceforSL"] = "0";
            ht["TrailingSL"] = "0";
            ht["LimitPriceforProfitOrder"] = "0";
            ht["ISTMOOrder"] = "Y";
            ht["ISCoverOrder"] = "N";
            ht["TriggerPriceSLforCoverOrder"] = "0";
            ht["TrailingSLforCoverOrder"] = "0";
            
            ht["TrailingSLForNormalOrder"] = "0";
            ht["TickSize"] = "0.05";
            ht["SourceAPP"] = "6";
            ht["SliceEnable"] = "N";


            var array = (from key in ht.AllKeys from value in ht.GetValues(key) select string.Format("{0}={1}", key, value)).ToArray();
            string orderstring = string.Join("&", array);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://trade.5paisa.com/Trade/Orders/OrderProceed?" + orderstring),
                Method = HttpMethod.Post,
            };
            var r = httpClient.Send(request);
            var strResponse = r.Content.ReadAsStringAsync().Result;
            SessionManager.Instance.AddMessage("*****************Start of Modify Order***************************");
            SessionManager.Instance.AddMessage(orderstring);
            SessionManager.Instance.AddMessage("------------------------Modify Order Response---------------------------------");
            SessionManager.Instance.AddMessage(strResponse);
            SessionManager.Instance.AddMessage("*****************End of Modify Order***************************");
        }

        
        private static string GetRequest(TradeCall tradeCall)
        {
            var doff = new DateTimeOffset(DateTime.Today);
            string today = doff.ToUnixTimeSeconds().ToString();

            NameValueCollection ht = new NameValueCollection();
            ht["RequestType"] = "P";
            ht["BuySell"] = tradeCall.OrderType;
            ht["Symbol"] = "";
            ht["FullName"] = tradeCall.ScriptName;
            ht["Name"] = tradeCall.ScriptName;
            ht["Category"] = "";
            ht["Quantity"] = tradeCall.Qty.ToString();
            ht["OldOrderNumber"] = "";
            ht["Exch"] = "N";
            ht["ExchType"] = "C";
            ht["Series"] = "";
            ht["DiscloseQty"] = "0";
            ht["CurrentPrice"] = tradeCall.Price.ToString();
            if (tradeCall.IsMarket)
            {
                ht["TriggerRate"] = "0";
                ht["isAtMarket"] = "true";
                ht["TriggerRateTMO"] = "0";
            }
            else
            {

                ht["TriggerRate"] = tradeCall.TriggerPrice.ToString();
                ht["isAtMarket"] = "false";
                ht["TriggerRateTMO"] = tradeCall.TriggerPrice.ToString();
            }
            ht["IOC"] = "false";
            ht["ISSL"] = "false";
            ht["ScripCode"] = tradeCall.ScriptCode.ToString();
            ht["TerminalId"] = "";
            ht["AfterHrs"] = "false";
            ht["SLStatus"] = "false";
            
            ht["sProduct"] = "";
            ht["Validity"] = "0";
            ht["ValideDate"] = "%2FDate(" + today + ")%2F";
            ht["disableBuySell"] = "false";
            ht["CallFrom"] = "";
            ht["currStatus"] = "";
            ht["TradedQty"] = "0";
            ht["smotrailsl"] = "";
            ht["Volume"] = "0";
            ht["AdvanceBuy"] = "false";
            ht["BidRate"] = "";
            ht["OffRate"] = "";
            ht["OrderValue"] = (tradeCall.Price * tradeCall.Qty).ToString();
            ht["ExchOrderID"] = "";
            ht["ExchOrderTime"] = "%2FDate(" + today + ")%2F";
            ht["AHPlaced"] = "false";
            ht["DelvIntra"] = "";
            // ht["LastRate"] = "0";
            // ht["LimitPriceforSL"] = "0";
            ht["TriggerPriceforSL"] = tradeCall.StopLossPrice.ToString();
            ht["TrailingSL"] = "0";
            ht["LimitPriceforProfitOrder"] = tradeCall.TargetPrice.ToString();
            ht["ISTMOOrder"] = "Y";
            ht["ISCoverOrder"] = "N";
            ht["TriggerPriceSLforCoverOrder"] = "0";
            ht["TrailingSLforCoverOrder"] = "0";
            
            ht["TrailingSLForNormalOrder"] = "0";
            ht["TickSize"] = "0.05";
            ht["SourceAPP"] = "6";
            ht["SliceEnable"] = "N";


            var array = (from key in ht.AllKeys from value in ht.GetValues(key) select string.Format("{0}={1}", key, value)).ToArray();
            return string.Join("&", array);

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