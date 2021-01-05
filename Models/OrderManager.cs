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
                WebSessionManager.PlaceOrder(tradeCall);
        }        
    }
}