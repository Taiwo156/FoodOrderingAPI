namespace APItask.Core.Models
{
    public class BankTransferDetails
    {
        public int Id { get; set; }
        public int PaymentAttemptId { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "NGN";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public PaymentAttempt PaymentAttempt { get; set; }
    }
}