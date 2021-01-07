using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Hangfire;
using System;
using System.Linq;

namespace My5Paisa.Models
{
    public static class OrderManager
    {
        private static List<string> trades = new List<string>();
        static OrderManager()
        {

        }

        public static void NewDay()
        {
            trades = new List<string>();
        }

        public static void Execute()
        {
            if (SessionManager.Instance.IsLive == false)
                return;
            bool loggedIn = false;
            foreach (var s in StrategyManager.AllStrategies)
            {
                foreach (var tc in s.Trades.Where(tc => tc.Status == TradeCallStatus.Triggered && tc.IsValid))
                {
                    if (trades.Count >= 10)
                        continue;
                    if (loggedIn == false)
                    {
                        WebSessionManager.Login();
                        loggedIn = true;
                    }
                    var c = tc.ScriptCode;
                    OpenPosition(tc);
                }
            }
        }

        public static void OpenPosition(TradeCall tradeCall)
        {
            if (trades.Contains(tradeCall.ScriptName))
            {
                tradeCall.Status = TradeCallStatus.Rejected;
                return;
            }
            bool isSuccess = WebSessionManager.PlaceOrder(tradeCall);
            if(isSuccess)
            {
                tradeCall.Status = TradeCallStatus.Executed;
            }
            else
            {
                tradeCall.Status = TradeCallStatus.Failed;
            }
            trades.Add(tradeCall.ScriptName);
        }
    }
}