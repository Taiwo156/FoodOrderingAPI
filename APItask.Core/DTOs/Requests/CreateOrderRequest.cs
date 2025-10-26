using System.ComponentModel.DataAnnotations;

namespace APItask.Core.DTOs.Requests
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public List<OrderItemRequest> OrderItems { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}