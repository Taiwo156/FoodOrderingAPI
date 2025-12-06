namespace Apitask.Core.DTOs.PaymentProviders
{
    //REQUEST:what we send to paystack for initializing payment
    public class PaystackInitializeRequest
    {
        public string email { get; set; }

        //amount in kobo
        public long amount { get; set; }
        public string reference { get; set; }
        public string callback_url { get; set; }
        public Dictionary<string, object> metadata { get; set; }
    }

    //RESPONSE:what paystack sends back after initialization
    public class PaystackInitializeResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public PaystackInitializeData data { get; set; }
    }

    //payment data returned by paystack
    public class PaystackInitializeData
    {
        //url redirects user to for payment
        //users enters card details on page
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }

    //verify response payment if payment is succeeded
    public class PaystackVerifyResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public PaystackVerifyData data { get; set; }
    }

    //Detailed payment verification data
    public class PaystackVerifyData
    {
        public string status { get; set; }
        public string reference { get; set; }
        public long amount { get; set; }
        public string gateway_response { get; set; }
        public DateTime? paid_at { get; set; }
        public PaystackCustomer customer { get; set; }
    }

    public class PaystackCustomer
    {
        public string email { get; set; }
        public string customer_code { get; set; }
    }
}