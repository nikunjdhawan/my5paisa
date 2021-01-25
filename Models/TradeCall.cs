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

        private bool isMarket = false;
        public bool IsMarket
        {
            get { return isMarket; }
            set { isMarket = value; }
        }

        private double ltp = 0;
        public double LTP
        {
            get { return ltp; }
            set { ltp = value; }
        }

        public static double stopLossPercent = 0.6;
        public static double targetPercent = 0.65;
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
                return (int)(100000 / Price);
            }
        }

        public static double GetStopLossPrice(double price_, bool isBuy)
        {
            if (isBuy)
            {
                return Math.Round(price_ * ((100 - stopLossPercent) / 100), 1);
            }
            else
            {
                return Math.Round(price_ * ((100 + stopLossPercent) / 100), 1);
            }
        }

        public double StopLossPrice
        {
            get
            {
                if (OrderType == "Buy")
                {
                    return GetStopLossPrice(Price, true);
                }
                if (OrderType == "Sell")
                {
                    return  GetStopLossPrice(Price, false);
                }
                return 0;

            }

        }

        public static double GetTargetPrice(double price_, bool isBuy)
        {
            if (isBuy)
            {
                return Math.Round(price_ * ((100 + targetPercent) / 100), 1);
            }
            else
            {
                return Math.Round(price_ * ((100 - targetPercent) / 100), 1);
            }
        }

        public double TargetPrice
        {
            get
            {
                if (OrderType == "Buy")
                {
                    return GetTargetPrice(Price, true);
                }
                if (OrderType == "Sell")
                {
                    return GetTargetPrice(Price, false);
                }
                return 0;

            }

        }

        public bool IsValid
        {
            get
            {
                if (OrderType == "Buy")
                {
                    if (TargetPrice < Price) return false;
                    if (StopLossPrice > Price) return false;
                    if (LTP < StopLossPrice) return false;
                    if (TriggerPrice > 0 && TriggerPrice - LTP < 1) return false;
                    if (LTP == 0) return false;
                }
                else
                {
                    if (TargetPrice > Price) return false;
                    if (StopLossPrice < Price) return false;
                    if (LTP > StopLossPrice) return false;
                    if (TriggerPrice > 0 && LTP - TriggerPrice < 1) return false;
                    if (LTP == 0) return false;
                }
                return true;
            }
        }
    }
}