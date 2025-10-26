namespace APItask.Core.Models
{
    public class PaymentRequest
    {
        public string Method { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "NGN";
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}