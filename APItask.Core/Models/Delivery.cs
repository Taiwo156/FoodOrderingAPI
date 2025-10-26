using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APItask.Core.Models
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryID { get; set; }

        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Default value

        [Required]
        public DateTime DeliveryDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Order? Order { get; set; }

        // Status constants for type safety
        public static class DeliveryStatus
        {
            public const string Pending = "Pending";
            public const string Shipped = "Shipped";
            public const string Delivered = "Delivered";
            public const string Cancelled = "Cancelled";
        }
    }
}