using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;

namespace My5Paisa.Models
{
    public static class NseQuoteProvider
    {
        private static List<Quote> quotes = new List<Quote>();
        public static Quote GetQuote(string scriptCode)
        {

            var cachedQuote = quotes.Where(q => q.ScriptCode == scriptCode).FirstOrDefault();
            if (cachedQuote != null)
                return cachedQuote;


            var client = new RestClient("https://www1.nseindia.com/live_market/dynaContent/live_watch/get_quote/ajaxGetQuoteJSON.jsp?symbol=JSWSTEEL&series=EQ");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Pragma", "no-cache");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
            request.AddHeader("X-Requested-With", "XMLHttpRequest");
            request.AddHeader("Sec-Fetch-Site", "same-origin");
            request.AddHeader("Sec-Fetch-Mode", "cors");
            request.AddHeader("Sec-Fetch-Dest", "empty");
            request.AddHeader("Referer", "https://www1.nseindia.com/live_market/dynaContent/live_watch/get_quote/GetQuote.jsp?symbol=JSWSTEEL");
            request.AddHeader("Accept-Language", "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7");
            request.AddHeader("Cookie", "");
            IRestResponse response = client.Execute(request);
            NseQuoteRoot resquote = JsonConvert.DeserializeObject(response.Content, typeof(NseQuoteRoot)) as NseQuoteRoot;
            var q = resquote.data[0];
            var quote = new Quote { Change = q.change, Close = q.closePrice, High = q.dayHigh, High52 = q.high52, LastPrice = q.lastPrice, Low = q.dayLow, Low52 = q.low52, Open = q.open, PClose = q.previousClose, PercentChange = q.pChange, ScriptCode = q.symbol };
            quotes.Add(quote);
            return quote;
        }
    }
}