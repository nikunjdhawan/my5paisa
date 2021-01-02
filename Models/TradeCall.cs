namespace My5Paisa.Models
{
    public class TradeCall
    {
        private double stopLossPercent = 0.5;
        private double targetPercent = 1;
        public int ScriptCode { get; set; }
        public string ScriptName { get; set; }
        public string OrderType { get; set; }
        public double Price { get; set; }

        public double StopLossPrice
        {
            get
            {
                if (OrderType == "Buy")
                {
                    return Price * ((100 - stopLossPercent) / 100);
                }
                if (OrderType == "Sell")
                {
                    return Price * ((100 + stopLossPercent) / 100);
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
                    return Price * ((100 + targetPercent) / 100);
                }
                if (OrderType == "Sell")
                {
                    return Price * ((100 - targetPercent) / 100);
                }
                return 0;

            }

        }

    }
}