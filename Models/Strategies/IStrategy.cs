using System.Collections.Generic;

namespace My5Paisa.Models
{
    public interface IStrategy
    {
        string CronExpression { get; }
        string Description { get; }
        void Run();
    }

    public static class StrategyManager
    {
        private static List<IStrategy> allStrategies = new List<IStrategy>();
        public static List<IStrategy> AllStrategies
        {
            get
            {
                return allStrategies;
            }
        }
        static StrategyManager()
        {
            allStrategies.Add(new MarketOpen());
        }
    }
}