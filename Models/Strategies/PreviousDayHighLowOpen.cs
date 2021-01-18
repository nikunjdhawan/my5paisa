using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;

namespace My5Paisa.Models
{
    public class PreviousDayHighLowOpen : StrategyBase
    {
        public override string ScanCronExpression
        {
            get
            {
                return "55 9 * * MON-FRI";
            }
        }

        public override string TriggerCronExpression
        {
            get
            {
                return "59 9 * * MON-FRI";
            }
        }

        public override string Description
        {
            get
            {
                return "Scans Nifty50 before the market opens and create trades based on previous day Hihg, Low and Open";
            }
        }

        public override string Name
        {
            get
            {
                return "Previous Open, High, Low";
            }
        }

        public override string Id
        {
            get
            {
                return "2";
            }
        }

        public override void Scan()
        {
            if (trades.Count > 0) return;
            var client = new RestClient("https://www1.nseindia.com/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("sec-fetch-dest", "document");
            request.AddHeader("sec-fetch-mode", "navigate");
            request.AddHeader("sec-fetch-site", "none");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);
            while (response.Content.StartsWith("{") == false)
            {
                response = client.Execute(request);
            }


            Nse1IndexRoot nifty50 = JsonConvert.DeserializeObject(response.Content, typeof(Nse1IndexRoot)) as Nse1IndexRoot;



            int buyOrdersCount = (int)(nifty50.advances / (nifty50.advances + nifty50.declines) * 10);

            foreach (var item in nifty50.data.Where(i => i.ltP < 5000 && i.ltP > 1000))
            {
                var diff = item.open - item.low;
                var diffP = diff / item.open * 100;
                if (diffP >= -0.1 && diffP < 0.1)
                {
                    TradeCall tc = new TradeCall { ScriptName = item.symbol, TriggerPrice = item.high , Price = item.high + 0.5, OrderType = "Buy", LTP = item.ltP };
                    trades.Add(tc);
                    MarketFeedManager.AddScript(tc.ScriptCode);
                }
            }
            foreach (var item in nifty50.data.Where(i => i.ltP < 5000 && i.ltP > 1000))
            {
                var diff = item.open - item.high;
                var diffP = diff / item.open * 100;
                if (diffP >= -0.1 && diffP < 0.1)
                {
                    TradeCall tc = new TradeCall { ScriptName = item.symbol, TriggerPrice = item.low, Price = item.low - .5, OrderType = "Sell", LTP = item.ltP };
                    trades.Add(tc);
                    MarketFeedManager.AddScript(tc.ScriptCode);
                }

            }
        }
    }
}