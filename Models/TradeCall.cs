using System;

namespace My5Paisa.Models
{
    public class TradeCall
    {
        private double stopLossPercent = 0.6;
        private double targetPercent = 1.2;
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

    }
}