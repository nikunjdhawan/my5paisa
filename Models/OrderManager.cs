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

        public static void OpenPosition(TradeCall tradeCall)
        {
            if(trades.Contains(tradeCall.ScriptName))
                return;
            if(trades.Count > 0)
                return;
            // if(tradeCall.ScriptName == "COALINDIA")
            if(SessionManager.Instance.IsLive)
            {
                WebSessionManager.PlaceOrder(tradeCall);
                trades.Add(tradeCall.ScriptName);
            }
        }        
    }
}