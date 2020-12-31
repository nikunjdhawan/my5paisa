// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System;

namespace My5Paisa.Models
{
    public class Body    {
        public string AllowBseCash { get; set; } 
        public string AllowBseDeriv { get; set; } 
        public string AllowBseMF { get; set; } 
        public string AllowMCXComm { get; set; } 
        public string AllowMcxSx { get; set; } 
        public string AllowNSECurrency { get; set; } 
        public string AllowNSEL { get; set; } 
        public string AllowNseCash { get; set; } 
        public string AllowNseComm { get; set; } 
        public string AllowNseDeriv { get; set; } 
        public string AllowNseMF { get; set; } 
        public int BulkOrderAllowed { get; set; } 
        public DateTime CleareDt { get; set; } 
        public string ClientCode { get; set; } 
        public string ClientName { get; set; } 
        public int ClientType { get; set; } 
        public string DemoTrial { get; set; } 
        public string EmailId { get; set; } 
        public string InteractiveLocalIP { get; set; } 
        public int InteractivePort { get; set; } 
        public string InteractivePublicIP { get; set; } 
        public int IsIDBound { get; set; } 
        public int IsIDBound2 { get; set; } 
        public string IsOnlyMF { get; set; } 
        public int IsPLM { get; set; } 
        public int IsPLMDefined { get; set; } 
        public DateTime LastAccessedTime { get; set; } 
        public string LastLogin { get; set; } 
        public DateTime LastPasswordModify { get; set; } 
        public string Message { get; set; } 
        public string OTPCredentialID { get; set; } 
        public int PLMsAllowed { get; set; } 
        public string POAStatus { get; set; } 
        public int PasswordChangeFlag { get; set; } 
        public string PasswordChangeMessage { get; set; } 
        public int RunningAuthorization { get; set; } 
        public DateTime ServerDt { get; set; } 
        public int Status { get; set; } 
        public int TCPBCastPort { get; set; } 
        public string TCPBcastLocalIP { get; set; } 
        public string TCPBcastPublicIP { get; set; } 
        public int UDPBCastPort { get; set; } 
        public string UDPBcastIP { get; set; } 
        public int VersionChanged { get; set; } 
    }

    public class Head    {
        public string responseCode { get; set; } 
        public string status { get; set; } 
        public string statusDescription { get; set; } 
    }

    public class LoginRoot    {
        public Body body { get; set; } 
        public Head head { get; set; } 
    }

}