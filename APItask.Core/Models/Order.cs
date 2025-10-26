using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APItask.Core.Models
{
    [Table("Order")]
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public virtual Users? User { get; set; }

        public void CalculateTotals()
        {
            SubTotal = OrderItems?.Sum(oi => oi.Quantity * oi.UnitPrice) ?? 0;
            Total = SubTotal + TaxAmount;
        }
    }
}