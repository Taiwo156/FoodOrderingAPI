using System;

namespace APItask.Core.Models
{
    public class PaymentAttempt
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Method { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}