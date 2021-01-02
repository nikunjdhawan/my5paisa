// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class Advance
    {
        public double declines { get; set; }
        public double advances { get; set; }
        public double unchanged { get; set; }
    }

    public class Meta
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string industry { get; set; }
        public List<string> activeSeries { get; set; }
        public List<string> debtSeries { get; set; }
        public List<string> tempSuspendedSeries { get; set; }
        public bool isFNOSec { get; set; }
        public bool isCASec { get; set; }
        public bool isSLBSec { get; set; }
        public bool isDebtSec { get; set; }
        public bool isSuspended { get; set; }
        public bool isETFSec { get; set; }
        public bool isDelisted { get; set; }
        public string isin { get; set; }
    }

    public class NseIndexDatum
    {
        public int priority { get; set; }
        public string symbol { get; set; }
        public string identifier { get; set; }
        public double open { get; set; }
        public double dayHigh { get; set; }
        public double dayLow { get; set; }
        public double lastPrice { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public double ffmc { get; set; }
        public double yearHigh { get; set; }
        public double yearLow { get; set; }
        public int totalTradedVolume { get; set; }
        public double totalTradedValue { get; set; }
        public string lastUpdateTime { get; set; }
        public double nearWKH { get; set; }
        public double nearWKL { get; set; }
        public double perChange365d { get; set; }
        public string date365dAgo { get; set; }
        public string chart365dPath { get; set; }
        public string date30dAgo { get; set; }
        public double perChange30d { get; set; }
        public string chart30dPath { get; set; }
        public string chartTodayPath { get; set; }
        public string series { get; set; }
        public Meta meta { get; set; }
    }

    public class Metadata
    {
        public string indexName { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double previousClose { get; set; }
        public double last { get; set; }
        public double percChange { get; set; }
        public double change { get; set; }
        public string timeVal { get; set; }
        public double yearHigh { get; set; }
        public double yearLow { get; set; }
        public int totalTradedVolume { get; set; }
        public double totalTradedValue { get; set; }
        public double ffmc_sum { get; set; }
    }

    public class MarketStatus
    {
        public string market { get; set; }
        public string marketStatus { get; set; }
        public string tradeDate { get; set; }
        public string index { get; set; }
        public double last { get; set; }
        public double variation { get; set; }
        public double percentChange { get; set; }
        public string marketStatusMessage { get; set; }
    }

    public class NseIndexRoot
    {
        public string name { get; set; }
        public Advance advance { get; set; }
        public string timestamp { get; set; }
        public List<NseIndexDatum> data { get; set; }
        public Metadata metadata { get; set; }
        public MarketStatus marketStatus { get; set; }
        public string date30dAgo { get; set; }
        public string date365dAgo { get; set; }
    }

}