// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class QuoteDatum
    {
        public string pricebandupper { get; set; }
        public string symbol { get; set; }
        public string applicableMargin { get; set; }
        public string bcEndDate { get; set; }
        public string totalSellQuantity { get; set; }
        public string adhocMargin { get; set; }
        public string companyName { get; set; }
        public string marketType { get; set; }
        public string exDate { get; set; }
        public string bcStartDate { get; set; }
        public string css_status_desc { get; set; }
        public double dayHigh { get; set; }
        public string basePrice { get; set; }
        public string securityVar { get; set; }
        public string pricebandlower { get; set; }
        public string sellQuantity5 { get; set; }
        public string sellQuantity4 { get; set; }
        public string sellQuantity3 { get; set; }
        public string cm_adj_high_dt { get; set; }
        public string sellQuantity2 { get; set; }
        public double dayLow { get; set; }
        public string sellQuantity1 { get; set; }
        public string quantityTraded { get; set; }
        public double pChange { get; set; }
        public string totalTradedValue { get; set; }
        public string deliveryToTradedQuantity { get; set; }
        public string totalBuyQuantity { get; set; }
        public string averagePrice { get; set; }
        public string indexVar { get; set; }
        public string cm_ffm { get; set; }
        public string purpose { get; set; }
        public string buyPrice2 { get; set; }
        public string secDate { get; set; }
        public string buyPrice1 { get; set; }
        public double high52 { get; set; }
        public double previousClose { get; set; }
        public string ndEndDate { get; set; }
        public double low52 { get; set; }
        public string buyPrice4 { get; set; }
        public string buyPrice3 { get; set; }
        public string recordDate { get; set; }
        public string deliveryQuantity { get; set; }
        public string buyPrice5 { get; set; }
        public string priceBand { get; set; }
        public string extremeLossMargin { get; set; }
        public string cm_adj_low_dt { get; set; }
        public string varMargin { get; set; }
        public string sellPrice1 { get; set; }
        public string sellPrice2 { get; set; }
        public string totalTradedVolume { get; set; }
        public string sellPrice3 { get; set; }
        public string sellPrice4 { get; set; }
        public string sellPrice5 { get; set; }
        public double change { get; set; }
        public string surv_indicator { get; set; }
        public string ndStartDate { get; set; }
        public string buyQuantity4 { get; set; }
        public bool isExDateFlag { get; set; }
        public string buyQuantity3 { get; set; }
        public string buyQuantity2 { get; set; }
        public string buyQuantity1 { get; set; }
        public string series { get; set; }
        public string faceValue { get; set; }
        public string buyQuantity5 { get; set; }
        public double closePrice { get; set; }
        public double open { get; set; }
        public string isinCode { get; set; }
        public double lastPrice { get; set; }
    }

    public class NseQuoteRoot
    {
        public string tradedDate { get; set; }
        public List<QuoteDatum> data { get; set; }
        public string optLink { get; set; }
        public List<string> otherSeries { get; set; }
        public string futLink { get; set; }
        public string lastUpdateTime { get; set; }
    }

}