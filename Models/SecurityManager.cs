using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace My5Paisa.Models
{
    public class SecurityManager
    {
        private static SecurityManager instance = null;
        public static SecurityManager Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = new SecurityManager();
                return instance;
            }
        }
        private int[] nifty50 = {467,236,526,547,694,881,910,1232,1330,1333,1348,1363,1394,1594,1624,1660,1922,2031,2475,2885,3045,3103,3351,3456,3499,3506,3787,4717,4963,5258,5900,7229,10604,10940,10999,11287,11483,11532,11536,11630,11723,13538,14977,15083,16669,16675,17963,20374,21808};
        List<Security> securities = null;
        private SecurityManager()
        {
            using (TextFieldParser parser = new TextFieldParser(@"wwwroot/Data/security.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                securities = new List<Security>();
                parser.ReadFields();
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    securities.Add(new Security(fields));
                }
            }
        }

        public int GetCode(string name)
        {
            var sec = GetNifty50.Where(n => n.Name == name).FirstOrDefault();
            if(sec != null)
                return sec.Scripcode;            
            else
                return securities.Where(n => n.Name == name).First().Scripcode;
        }

        private List<Security> nifty50list = null;

        public List<Security> GetNifty50
        {
            get
            {
                if (nifty50list == null)
                    nifty50list = this.securities.Where(s => s.Exch == "N" && s.ExchType == "C" && s.Series == "EQ" && nifty50.Contains(s.Scripcode)).ToList();
                return nifty50list;
            }
        }
    }
    public class Security
    {

        public Security(string[] args)
        {
            
            this.Exch = args[0];
            this.ExchType = args[1];
            this.Scripcode = int.Parse(args[2]);
            this.Name = args[3];
            this.Series = args[4];
            this.Expiry = args[5];
            this.CpType = args[6];
            this.StrikeRate = args[7];
            this.Category = args[8];
            this.ISIN = args[9];
            this.FullName = args[10];
        }
        public string Exch { get; set; }
        public string ExchType { get; set; }
        public int Scripcode { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Expiry { get; set; }
        public string CpType { get; set; }
        public string StrikeRate { get; set; }
        public string Category { get; set; }
        public string ISIN { get; set; }
        public string FullName { get; set; }
    }
}