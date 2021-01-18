using System;

namespace My5Paisa.Models
{
    public enum TradeCallStatus
    {
        Pending = 1,
        Triggered,
        Failed,
        Executed,
        Rejected
    }
    public class TradeCall
    {
        private TradeCallStatus status = TradeCallStatus.Pending;
        public TradeCallStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        
        private double triggerPrice = 0;
        public double TriggerPrice
        {
            get { return triggerPrice; }
            set { triggerPrice = value; }
        }

        private double ltp = 0;
        public double LTP
        {
            get { return ltp; }
            set { ltp = value; }
        }
        
        private double stopLossPercent = 0.6;
        private double targetPercent = 0.65;
        private int scriptCode = -1;
        public int ScriptCode
        {
            get
            {
                return (scriptCode == -1) ? SecurityManager.Instance.GetCode(ScriptName) : scriptCode;
            }
            set { scriptCode = value; }
        }

        public string ScriptName { get; set; }
        public string OrderType { get; set; }

        private double price;
        public double Price
        {
            get { return Math.Round(price, 1); }
            set { price = value; }
        }

        public int Qty
        {
            get
            {
                return (int)(10000 / Price);
            }
        }

        public double StopLossPrice
        {
            get
            {
                if (OrderType == "Buy")
                {
                    return Math.Round(Price * ((100 - stopLossPercent) / 100), 1);
                }
                if (OrderType == "Sell")
                {
                    return Math.Round(Price * ((100 + stopLossPercent) / 100), 1);
                }
                return 0;

            }

        }

        public double TargetPrice
        {
            get
            {
                if (OrderType == "Buy")
                {
                    return Math.Round(Price * ((100 + targetPercent) / 100), 1);
                }
                if (OrderType == "Sell")
                {
                    return Math.Round(Price * ((100 - targetPercent) / 100), 1);
                }
                return 0;

            }

        }

        public bool IsValid
        {
            get
            {
                if(OrderType == "Buy")
                {
                    if(TargetPrice < Price) return false;
                    if(StopLossPrice > Price) return false;
                    if(LTP < StopLossPrice) return false;
                    if(TriggerPrice>0 && TriggerPrice - LTP < 1) return false; 
                    if(LTP == 0) return false;
                }
                else
                {
                    if(TargetPrice > Price) return false;
                    if(StopLossPrice < Price) return false;
                    if(LTP > StopLossPrice) return false;
                    if(TriggerPrice>0 && LTP - TriggerPrice < 1) return false;
                    if(LTP == 0) return false;
                }
                return true;
            }
        }
    }
}