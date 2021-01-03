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
        private OrderManager()
        {

        }
        private static OrderManager instance = null;
        public static OrderManager Instance
        {
            get
            {
                lock (typeof(OrderManager))
                {
                    if (instance != null) return instance;
                    instance = new OrderManager();
                    return instance;
                }
            }
        }

        double targetPercent = 1;
        public void OpenPosition(TradeCall tradeCall)
        {
            if(SessionManager.Instance.IsLive)
                PlaceOrder(tradeCall.ScriptCode, tradeCall.OrderType, tradeCall.Price, tradeCall.StopLossPrice);
        }

        private bool PlaceOrder(int scriptCode, string orderType, double price, double stopLoss = 0)
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
            BalanceOrders();

            return brokerOrderId > 0;
        }

        private void CancelOrder(OrderBookDetail obd)
        {
            var doff = new DateTimeOffset(DateTime.Today);
            string today = doff.ToUnixTimeSeconds().ToString();
            string tomorrow = doff.AddDays(1).ToUnixTimeSeconds().ToString();
            var ordertype = (obd.BuySell == "S") ? "SELL" : "BUY";
            var client = new RestClient("https://Openapi.5paisa.com/VendorsAPI/Service1.svc/V1/OrderRequest");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "PIData=TklLVU5K; 5paisacookie=zdw053xuljn0d5q4potp5djs");
            request.AddParameter("application/json", "{\r\n\t\"head\": {\r\n        \"appName\": \"5P54965884\",\r\n        \"appVer\": \"1.0\",\r\n        \"key\": \"PNC67ejiGYsWDAXvxEVVORSHurKnExho\",\r\n        \"osName\": \"WEB\",\r\n        \"requestCode\": \"5POrdReq\",\r\n        \"userId\": \"m5rK5jEwGtK\",\r\n        \"password\": \"Vw0EUSzdh6P\"\r\n    },\r\n\t\"body\": {\r\n\t\t\"ClientCode\": \"54965884\",\r\n\t\t\"OrderFor\": \"C\",\r\n\t\t\"Exchange\": \"N\",\r\n\t\t\"ExchangeType\": \"C\",\r\n\t\t\"Price\": " + obd.Rate + ",\r\n\t\t\"OrderID\": 0,\r\n\t\t\"OrderType\": \"" + ordertype + "\",\r\n\t\t\"Qty\": " + obd.Qty + ",\r\n\t\t\"OrderDateTime\": \"/Date(" + today + ")/\",\r\n\t\t\"ScripCode\": " + obd.ScripCode + ",\r\n\t\t\"AtMarket\": false,\r\n\t\t\"RemoteOrderID\": \"1\",\r\n\t\t\"ExchOrderID\": " + obd.ExchOrderID + ",\r\n\t\t\"DisQty\": 0,\r\n\t\t\"IsStopLossOrder\": false,\r\n\t\t\"StopLossPrice\": 0,\r\n\t\t\"IsVTD\": false,\r\n\t\t\"IOCOrder\": false,\r\n\t\t\"IsIntraday\": true,\r\n\t\t\"PublicIP\": \"182.69.74.25\",\r\n\t\t\"AHPlaced\": \"N\",\r\n\t\t\"ValidTillDate\": \"/Date(" + tomorrow + ")/\",\r\n\t\t\"iOrderValidity\": 0,\r\n\t\t\"TradedQty\": 0,\r\n\t\t\"OrderRequesterCode\": \"54965884\",\r\n\t\t\"AppSource\": 4498\r\n\t}\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        public void BalanceOrders()
        {
            var positions = SessionManager.Instance.GetNetPositions();
            if (positions == null || positions.body.NetPositionDetail.Count == 0)
                return;

            var orders = SessionManager.Instance.GetOrderBook();

            foreach (var position in positions.body.NetPositionDetail)
            {
                if (position.NetQty != 0)
                {
                    var scriptOrders = orders.body.OrderBookDetail.Where(obd => obd.OrderStatus == "Pending" && obd.ScripCode == position.ScripCode).ToList();
                    if (scriptOrders.Count == 0)
                    {
                        double target = 0;
                        if (position.BuyQty > 0)
                        {
                            target = Math.Round(position.BuyAvgRate * ((100 + targetPercent) / 100), 1);
                            PlaceOrder(position.ScripCode, "Sell", target);
                        }
                        if (position.SellQty > 0)
                        {
                            target = Math.Round(position.SellAvgRate * ((100 - targetPercent) / 100), 1);
                            PlaceOrder(position.ScripCode, "Buy", target);
                        }
                        SessionManager.Instance.AddMessage("Opened the Pending order for Target hit for " + position.ScripName);
                    }


                }
                else
                {
                    var scriptOrders = orders.body.OrderBookDetail.Where(obd => obd.OrderStatus == "Pending" && obd.ScripCode == position.ScripCode).ToList();
                    if (scriptOrders.Count > 0)
                    {
                        //Cancel the pending Target Order in case stop loss is hit
                        CancelOrder(scriptOrders.First());
                        SessionManager.Instance.AddMessage("Cancelled the Pending order after SL hit for " + position.ScripName);
                    }

                }
            }
        }
    }


}