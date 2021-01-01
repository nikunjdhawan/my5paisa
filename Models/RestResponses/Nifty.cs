using System;
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class Nifty50Datum    {
        public string symbol { get; set; } 
        public string series { get; set; } 
        public string xDt { get; set; } 
        public string caAct { get; set; } 
        public double iep { get; set; } 
        public double chn { get; set; } 
        public double perChn { get; set; } 
        public double pCls { get; set; } 
        public string trdQnty { get; set; } 
        public double iVal { get; set; } 
        public double mktCap { get; set; } 
        public double yHigh { get; set; } 
        public double yLow { get; set; } 
        public double sumVal { get; set; } 
        public string sumQnty { get; set; } 
        public string finQnty { get; set; } 
        public string sumfinQnty { get; set; } 
    }

    public class Nifty50Root    {
        public double declines { get; set; } 
        public double noChange { get; set; } 
        public List<Nifty50Datum> data { get; set; } 
        public double advances { get; set; } 
    }
}