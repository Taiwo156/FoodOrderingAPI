namespace APItask.Core.Models
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public string AuthorizationUrl { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
    }
}