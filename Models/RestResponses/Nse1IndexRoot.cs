
using System.Collections.Generic;

namespace My5Paisa.Models
{    
    public class Nse1IndexDatum    
    {
        public string symbol { get; set; } 
        public double open { get; set; } 
        public double high { get; set; } 
        public double low { get; set; } 
        public double ltP { get; set; } 
        public double ptsC { get; set; } 
        public double per { get; set; } 
        public double trdVol { get; set; } 
        public double trdVolM { get; set; } 
        public double ntP { get; set; } 
        public double mVal { get; set; } 
        public double wkhi { get; set; } 
        public double wklo { get; set; } 
        public double wkhicm_adj { get; set; } 
        public double wklocm_adj { get; set; } 
        public string xDt { get; set; } 
        public string cAct { get; set; } 
        public double previousClose { get; set; } 
        public string dayEndClose { get; set; } 
        public double iislPtsChange { get; set; } 
        public double iislPercChange { get; set; } 
        public double yPC { get; set; } 
        public double mPC { get; set; } 
    }

    public class Nse1IndexLatestData    {
        public string indexName { get; set; } 
        public string open { get; set; } 
        public string high { get; set; } 
        public string low { get; set; } 
        public string ltp { get; set; } 
        public string ch { get; set; } 
        public string per { get; set; } 
        public string yCls { get; set; } 
        public string mCls { get; set; } 
        public string yHigh { get; set; } 
        public string yLow { get; set; } 
    }

    public class Nse1IndexRoot    {
        public double declines { get; set; } 
        public List<Nse1IndexDatum> data { get; set; } 
        public string trdVolumesum { get; set; } 
        public List<Nse1IndexLatestData> latestData { get; set; } 
        public double advances { get; set; } 
        public int unchanged { get; set; } 
        public string trdValueSumMil { get; set; } 
        public string time { get; set; } 
        public string trdVolumesumMil { get; set; } 
        public string trdValueSum { get; set; } 
    }

}