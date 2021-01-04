// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System;
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class EquityMargin    {
        public double ALB { get; set; } 
        public int Adhoc { get; set; } 
        public double AvailableMargin { get; set; } 
        public int GHV { get; set; } 
        public int GHVPer { get; set; } 
        public double GrossMargin { get; set; } 
        public int Lb { get; set; } 
        public double Mgn4PendOrd { get; set; } 
        public double Mgn4Position { get; set; } 
        public int NDDebit { get; set; } 
        public int OptionsMtoMLoss { get; set; } 
        public int PDHV { get; set; } 
        public int Payments { get; set; } 
        public int Receipts { get; set; } 
        public int THV { get; set; } 
        public int UnclChq { get; set; } 
        public int Undlv { get; set; } 
    }

    public class MarginBody    {
        public string ClientCode { get; set; } 
        public List<EquityMargin> EquityMargin { get; set; } 
        public string Message { get; set; } 
        public int Status { get; set; } 
        public DateTime TimeStamp { get; set; } 
    }

    public class MarginHead    {
        public string responseCode { get; set; } 
        public string status { get; set; } 
        public string statusDescription { get; set; } 
    }

    public class MarginRoot    {
        public MarginBody body { get; set; } 
        public MarginHead head { get; set; } 
    }

}