// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System;
using System.Linq;
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class NetPositionDetail
    {
        public int BodQty { get; set; }
        public double BookedPL { get; set; }
        public double BuyAvgRate { get; set; }
        public int BuyQty { get; set; }
        public double BuyValue { get; set; }
        public string Exch { get; set; }
        public string ExchType { get; set; }
        public double LTP { get; set; }
        public double MTOM { get; set; }
        public int Multiplier { get; set; }
        public int NetQty { get; set; }
        public string OrderFor { get; set; }
        public double PreviousClose { get; set; }
        public int ScripCode { get; set; }
        public string ScripName { get; set; }
        public double SellAvgRate { get; set; }
        public int SellQty { get; set; }
        public double SellValue { get; set; }
    }

    public class NetPositionBody
    {
        public string Message { get; set; }
        public List<NetPositionDetail> NetPositionDetail { get; set; }
        public int Status { get; set; }
    }

    public class NetPositionHead
    {
        public string responseCode { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
    }

    public class NetPositionRoot
    {
        public NetPositionBody body { get; set; }
        public NetPositionHead head { get; set; }

        public double BookedPL
        {
            get
            {
                if (this.body.NetPositionDetail.Count == 0)
                    return 0;
                double bookedPL = body.NetPositionDetail.Sum(x => x.BookedPL);
                return bookedPL;
            }
        }

        public double UnBookedPL
        {
            get
            {
                if (this.body.NetPositionDetail.Count == 0)
                    return 0;
                double bookedPL = body.NetPositionDetail.Sum(x => x.MTOM);
                return bookedPL;
            }
        }

        public double TotalPL
        {
            get
            {
                return BookedPL + UnBookedPL;
            }
        }
    }

}