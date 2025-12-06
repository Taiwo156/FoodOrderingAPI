using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Core.DTOs.PaymentProviders
{
    //request: what we send to flutterwave
    public class FlutterwaveInitializeRequest
    {
        // transaction reference (MUST be unique)
        //format: FLW-{timestamp}-{random}
        public string tx_ref { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string redirect_url { get; set; }
        public FlutterwaveCustomer customer { get; set; }
        public Dictionary<string, object> metadata { get; set; }
        public string payment_options { get; set; }
    }

    //customer information for flutterwave
    public class FlutterwaveCustomer
    {
        public string email { get; set; }
        public string phonenumber { get; set; }
        public string name { get; set; }
    }

    
    public class FlutterwaveInitializeResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public FlutterwaveInitializeData data { get; set; }
    }

    //Payment link data
    public class FlutterwaveInitializeData
    {
        public string link { get; set; }
    }

    //verify: payment verification response
    public class FlutterwaveVerificationResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public FlutterwaveVerifyData data { get; set; }
    }

    //detailed verification data
    public class FlutterwaveVerifyData
    {
        public string status { get; set; }
        public string tx_ref { get; set; }
        public long id { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public FlutterwaveCustomer customer { get; set; }

    }
}
