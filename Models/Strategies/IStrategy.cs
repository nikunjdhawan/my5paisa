using System.Collections.Generic;
using System.Linq;

namespace My5Paisa.Models
{
    public abstract class StrategyBase
    {
        public abstract string ScanCronExpression { get; }
        public abstract string ExecuteCronExpression { get; }
        public abstract string Description { get; }
        public abstract string Name { get; }
        public abstract string Id { get; }


        protected List<TradeCall> trades = new List<TradeCall>();
        public List<TradeCall> Trades
        {
            get
            {
                return trades;

            }
        }

        public virtual void Execute()
        {
            WebSessionManager.Login();
            foreach (var tc in trades)
            {
                var c = tc.ScriptCode;
                OrderManager.OpenPosition(tc);
            }
            trades.Clear();
        }

        public abstract void Scan();
    }

    public static class StrategyManager
    {
        private static List<StrategyBase> allStrategies = new List<StrategyBase>();
        public static List<StrategyBase> AllStrategies
        {
            get
            {
                return allStrategies;
            }
        }
        static StrategyManager()
        {
            allStrategies.Add(new MarketOpen());
            allStrategies.Add(new PreviousDayHighLowOpen());
        }

        public static StrategyBase GetById(string id)
        {
            return allStrategies.Where(s => s.Id == id).FirstOrDefault();
        }
    }
}