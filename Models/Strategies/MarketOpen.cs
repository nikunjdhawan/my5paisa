using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace My5Paisa.Models
{
    public class MarketOpen : IStrategy
    {
        public string CronExpression
        {
            get
            {
                return "8 9 * * MON-FRI";
            }
        }

        public string Description
        {
            get
            {
                return "Scans Nifty50 before the market opens and create trades in ration of market strength";
            }
        }

        public void Run()
        {
            if(SessionManager.Instance.Trades.Count>0) return;
            var client = new RestClient("https://www1.nseindia.com/live_market/dynaContent/live_analysis/pre_open/nifty.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Accept-Language", "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);
            Nifty50Root nifty50 = JsonConvert.DeserializeObject(response.Content, typeof(Nifty50Root)) as Nifty50Root;

            int buyOrdersCount = (int)(nifty50.advances / (nifty50.advances + nifty50.declines) * 10);

            foreach (var item in nifty50.data.Where(i => i.perChn > 0 && i.iep < 5000).OrderBy(i => i.perChn).Take(buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Buy"};
                Buy(tc);
            }
            foreach (var item in nifty50.data.Where(i => i.perChn < 0 && i.iep < 5000).OrderByDescending(i => i.perChn).Take(10 - buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Sell"};
                Sell(tc);
            }
        }

        private static void Buy(TradeCall tc)
        {
            SessionManager.Instance.Trades.Add(tc);
            SessionManager.Instance.AddMessage("Buy: " + tc.ScriptName + " at " + tc.Price.ToString("f") + " Take Profit at: " + tc.TargetPrice.ToString("c") + " Stop Loss at: " + tc.StopLossPrice.ToString("c"));
        }

        private static void Sell(TradeCall tc)
        {
            SessionManager.Instance.Trades.Add(tc);
            SessionManager.Instance.AddMessage("Buy: " + tc.ScriptName + " at " + tc.Price.ToString("f") + " Take Profit at: " + tc.TargetPrice.ToString("c") + " Stop Loss at: " + tc.StopLossPrice.ToString("c"));
        }
    }
}