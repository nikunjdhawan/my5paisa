using System;
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class OrderBookDetail
    {
        public string AHProcess { get; set; }
        public string AfterHours { get; set; }
        public string AtMarket { get; set; }
        public int BrokerOrderId { get; set; }
        public DateTime BrokerOrderTime { get; set; }
        public string BuySell { get; set; }
        public string DelvIntra { get; set; }
        public int DisClosedQty { get; set; }
        public string Exch { get; set; }
        public string ExchOrderID { get; set; }
        public DateTime ExchOrderTime { get; set; }
        public string ExchType { get; set; }
        public int MarketLot { get; set; }
        public int OldorderQty { get; set; }
        public string OrderRequesterCode { get; set; }
        public string OrderStatus { get; set; }
        public string OrderValidUpto { get; set; }
        public int OrderValidity { get; set; }
        public int PendingQty { get; set; }
        public int Qty { get; set; }
        public double Rate { get; set; }
        public string Reason { get; set; }
        public string RequestType { get; set; }
        public double SLTriggerRate { get; set; }
        public string SLTriggered { get; set; }
        public int SMOProfitRate { get; set; }
        public int SMOSLLimitRate { get; set; }
        public int SMOSLTriggerRate { get; set; }
        public int SMOTrailingSL { get; set; }
        public int ScripCode { get; set; }
        public string ScripName { get; set; }
        public int TerminalId { get; set; }
        public int TradedQty { get; set; }
        public string WithSL { get; set; }
    }

    public class OrderBookBody
    {
        public string Message { get; set; }
        public List<OrderBookDetail> OrderBookDetail { get; set; }
        public int Status { get; set; }
    }

    public class OrderBookHead
    {
        public string responseCode { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
    }

    public class OrderBookRoot
    {
        public OrderBookBody body { get; set; }
        public OrderBookHead head { get; set; }
    }

}