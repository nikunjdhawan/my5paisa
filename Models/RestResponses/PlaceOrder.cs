// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System;
using System.Collections.Generic;

namespace My5Paisa.Models
{
    public class OrderResponseBody
    {
        public int BrokerOrderID { get; set; }
        public string ClientCode { get; set; }
        public string Exch { get; set; }
        public string ExchOrderID { get; set; }
        public string ExchType { get; set; }
        public int LocalOrderID { get; set; }
        public string Message { get; set; }
        public int RMSResponseCode { get; set; }
        public int ScripCode { get; set; }
        public int Status { get; set; }
        public DateTime Time { get; set; }
    }

    public class OrderResponseHead
    {
        public string responseCode { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
    }

    public class OrderResponseRoot
    {
        public OrderResponseBody body { get; set; }
        public OrderResponseHead head { get; set; }
    }

}