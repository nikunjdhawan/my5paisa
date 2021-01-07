using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace My5Paisa.Models
{
    public class MarketOpen : StrategyBase
    {
        public override string ScanCronExpression
        {
            get
            {
                return "9 9 * * MON-FRI";
            }
        }

        public override string TriggerCronExpression
        {
            get
            {
                return "12 9 * * MON-FRI";
            }
        }

        public override string Description
        {
            get
            {
                return "Scans Nifty50 before the market opens and create trades in ration of market strength";
            }
        }

        public override string Name
        {
            get
            {
                return "Pre - Market Open";
            }
        }

        public override string Id
        {
            get
            {
                return "1";
            }
        }

        public override void Scan()
        {
            if(trades.Count>0) return;
            var client = new RestClient("https://www1.nseindia.com/live_market/dynaContent/live_analysis/pre_open/nifty.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("Accept-Language", "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);
            Nifty50Root nifty50 = JsonConvert.DeserializeObject(response.Content, typeof(Nifty50Root)) as Nifty50Root;

            int buyOrdersCount = (int)(nifty50.advances / (nifty50.advances + nifty50.declines) * 10);

            foreach (var item in nifty50.data.Where(i => i.perChn > 0 && i.iep < 5000 && i.iep > 1000).OrderBy(i => i.perChn).Take(buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Buy"};
                trades.Add(tc);
            }
            foreach (var item in nifty50.data.Where(i => i.perChn < 0 && i.iep < 5000 && i.iep > 1000).OrderByDescending(i => i.perChn).Take(10 - buyOrdersCount))
            {
                TradeCall tc = new TradeCall{ScriptName = item.symbol, Price = item.iep, OrderType = "Sell"};
                trades.Add(tc);
            }
        }
    }
}