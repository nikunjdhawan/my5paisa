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
                return "0 8 * * MON-FRI";
            }
        }

        public override string ExecuteCronExpression
        {
            get
            {
                return "16 9 * * MON-FRI";
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
            // var httpClient = new HttpClient();
            // string resp = httpClient.GetStringAsync("https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%2050").Result;
            if (trades.Count > 0) return;
            var client = new RestClient("https://www.nseindia.com/api/equity-stockIndices?index=NIFTY%2050");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";
            request.AddHeader("accept-language", "en-US,en;q=0.9");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);


            NseIndexRoot nifty50 = JsonConvert.DeserializeObject(response.Content, typeof(NseIndexRoot)) as NseIndexRoot;



            int buyOrdersCount = (int)(nifty50.advance.advances / (nifty50.advance.advances + nifty50.advance.declines) * 10);

            foreach (var item in nifty50.data.Where(i => i.open == i.dayLow && i.lastPrice < 5000).Take(buyOrdersCount))
            {
                TradeCall tc = new TradeCall { ScriptName = item.symbol, Price = item.dayHigh, OrderType = "Buy" };
                trades.Add(tc);
            }
            foreach (var item in nifty50.data.Where(i => i.open == i.dayHigh && i.lastPrice < 5000).Take(10 - buyOrdersCount))
            {
                TradeCall tc = new TradeCall { ScriptName = item.symbol, Price = item.dayLow, OrderType = "Sell" };
                trades.Add(tc);
            }
        }
    }
}