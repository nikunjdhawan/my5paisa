using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Hangfire;
using System;
using System.Linq;

namespace My5Paisa.Models
{
    public class OrderManager
    {
        public void OpenPosition(string scriptCode, string orderType, double price, double stopLossPercent = 1, double targetPercent = 1)
        {
            double stopLoss = 0;
            double target = 0;
            if (orderType == "Buy")
            {
                stopLoss = price * ((100 - stopLossPercent) / 100);
                target = price * ((100 + targetPercent) / 100);
            }
            else
            {
                stopLoss = price * ((100 + stopLossPercent) / 100);
                target = price * ((100 - targetPercent) / 100);
            }

            PlaceOrder(scriptCode, orderType, price, stopLoss);

            if (orderType == "Buy")
                PlaceOrder(scriptCode, "Sell", target);
            else
                PlaceOrder(scriptCode, "Buy", target);



        }

        private bool PlaceOrder(string scriptCode, string orderType, double price, double stopLoss = 0)
        {
            var doff = new DateTimeOffset(DateTime.Today);
            string today = doff.ToUnixTimeSeconds().ToString();
            string tomorrow = doff.AddDays(1).ToUnixTimeSeconds().ToString();
            var isStopLoss = (stopLoss > 0) ? "true" : "false";

            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V1/OrderRequest");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs");
            request.AddParameter("application/json", "{\r\n\t\"head\": {\r\n        \"appName\": \"5P54965884\",\r\n        \"appVer\": \"1.0\",\r\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\r\n        \"osName\": \"WEB\",\r\n        \"requestCode\": \"5POrdReq\",\r\n        \"userId\": \"m5rK5jEwGtK\",\r\n        \"password\": \"Vw0EUSzdh6P\"\r\n    },\r\n\t\"body\": {\r\n\t\t\"ClientCode\": \"54965884\",\r\n\t\t\"OrderFor\": \"P\",\r\n\t\t\"Exchange\": \"N\",\r\n\t\t\"ExchangeType\": \"C\",\r\n\t\t\"Price\": " + price + ",\r\n\t\t\"OrderID\": 0,\r\n\t\t\"OrderType\": \"" + orderType.ToUpper() + "\",\r\n\t\t\"Qty\": 1,\r\n\t\t\"OrderDateTime\": \"/Date(" + today + ")/\",\r\n\t\t\"ScripCode\": " + scriptCode + ",\r\n\t\t\"AtMarket\": false,\r\n\t\t\"RemoteOrderID\": \"1\",\r\n\t\t\"ExchOrderID\": 0,\r\n\t\t\"DisQty\": 0,\r\n\t\t\"IsStopLossOrder\": " + isStopLoss + ",\r\n\t\t\"StopLossPrice\": " + stopLoss + ",\r\n\t\t\"IsVTD\": false,\r\n\t\t\"IOCOrder\": false,\r\n\t\t\"IsIntraday\": true,\r\n\t\t\"PublicIP\": \"182.69.74.25\",\r\n\t\t\"AHPlaced\": \"N\",\r\n\t\t\"ValidTillDate\": \"/Date(" + tomorrow + ")/\",\r\n\t\t\"iOrderValidity\": 0,\r\n\t\t\"TradedQty\": 0,\r\n\t\t\"OrderRequesterCode\": \"54965884\",\r\n\t\t\"AppSource\": 4498\r\n\t}\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            OrderResponseRoot orderresponse = JsonConvert.DeserializeObject(response.Content, typeof(OrderResponseRoot)) as OrderResponseRoot;
            int brokerOrderId = orderresponse.body.BrokerOrderID;

            return brokerOrderId > 0;
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
            OrderBookRoot orderBook =  JsonConvert.DeserializeObject(response.Content, typeof(OrderBookRoot)) as OrderBookRoot;
            Console.WriteLine(response.Content);
        }

        public void CloseResiduals()
        {
            var positions = SessionManager.Instance.GetNetPositions();
            if (positions == null || positions.body.NetPositionDetail.Count == 0)
                return;

        }
    }


}