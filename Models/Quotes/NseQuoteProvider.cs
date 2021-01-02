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
            // request.AddHeader("Cookie", "ak_bmsc=CC4D18517B82AE87FFE5F69FAE4924FE17394A50782E0000CD0EF05F9650ED46~plchnxWt/borfB8v/oG7/nkJbiyVeArY//TLhVggbPxVAQBvX4Zst23b+6to79Q7yN86UsRP2do9V9AUgRo/ytMI+0dSIJgMJmIayQCX94Om3hhNpaaF8OA0DkKmkFARtAvmeAT3Xfm+9QGFxkUWseb6MSGYn30u2YEUEAvq0P1Aj9HQdX+13elRYw64kNqQ1mBce8xncTahFhnWsv5oeF76/SdlG3fzt1aF8TB3+UvlZFMvAmoXUA638KTgPNWcPH; _ga=GA1.2.2100928855.1609567956; _gid=GA1.2.1660714099.1609567956; pointer=1; sym1=JSWSTEEL; NSE-TEST-1=1910513674.20480.0000; RT=\"z=1&dm=nseindia.com&si=047912a9-fff0-434c-8646-30ead19b10ea&ss=kjfbszkn&sl=1&tt=6tv&bcn=%2F%2F684d0d36.akstat.io%2F&ld=1wsfh\"; JSESSIONID=FA481A9D9F1EE90F75F6D04A7941A22C.jvm1; bm_sv=A1DB8C8B7284249C92802E7D9FE33A5F~WFwGWwkNrLqvieO5X+eK9vaV9ShciLQO0/MvBVHNuDkdMpZwQUQ8jGElBqf/2StnlyDrwAhREUvKlH0gsScnsfzwHthV7sLtFbxulf29IITVUFBUdu49U3/jV5543swurPXDbJeNzJybtUHjvXkTdu+Mr2fb4X7GS36M+mREnKw=; NSE-TEST-1=1944068106.20480.0000; ak_bmsc=846E28BAF3E6B9E2DB422AE5603CD7AB6011B68D521400002D12F05F739A654B~plszr9UdqjFePtR4nuFy3GEuT17NIWE2LKTgNYWI84wFGUkM8B8kzVnWZphBA55TzPmwdiw63lWTzqba+uX3bTaiLbrBQr1Jmno6cVefEoasTKBpT1M2hqq/dwGYqwKVq1UtsumfQCuSElXYuw0YCUsJppNbELTv60290pd1hfTvpwKuIDkLd7d7KK43EBwW+1xq175c1F7aXkNLxjm6XyfvyiA/AtGhBU7KZxd7nYcEk=; bm_mi=5EA3610745A19948005E8F00E15F643E~3scT9pRAz3xGupXg9+vRkNWB/1E5YgJifkSTzmDWzuNpiQ1G4tj5N3zyTp3WTfxQauHb0cznbAHQeGv7H+M6IKNmokKEHAHBF5cFskhrieixROZycFejGloePinjkYNZqyIkA/mU19P6aMbe6ucdGcvJoqfMTFxAHgIeyWEwTPUf543t7+/VBDEoIwlM4DMiPjQ7chG6zoAZG53vqwevz+5PoDBasnU8Cy2pfo3qO2doWWRnSM0IEac9UCf3HkoPoFK88Hxz/zhbqaS+aOzlfxnlZL9IhCT5tPs1Ix04xBwSovbsiecyZ4k2LMmxE0m+; JSESSIONID=C9AEE6C0737C4FDE9ECB1B0D90567433.tomcat2; bm_sv=A1DB8C8B7284249C92802E7D9FE33A5F~WFwGWwkNrLqvieO5X+eK9vaV9ShciLQO0/MvBVHNuDkdMpZwQUQ8jGElBqf/2StnlyDrwAhREUvKlH0gsScnsfzwHthV7sLtFbxulf29IITQl5VTjnfkz/oa5dicRhXWDyDuhsFyyEfoSCJsHwgVxDfkjpOG5i545h2Ze6BBl7c=");
            IRestResponse response = client.Execute(request);
            NseQuoteRoot resquote = JsonConvert.DeserializeObject(response.Content, typeof(NseQuoteRoot)) as NseQuoteRoot;
            var q = resquote.data[0];
            var quote = new Quote { Change = q.change, Close = q.closePrice, High = q.dayHigh, High52 = q.high52, LastPrice = q.lastPrice, Low = q.dayLow, Low52 = q.low52, Open = q.open, PClose = q.previousClose, PercentChange = q.pChange, ScriptCode = q.symbol };
            quotes.Add(quote);
            return quote;
        }
    }
}