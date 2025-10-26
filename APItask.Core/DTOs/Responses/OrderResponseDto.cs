namespace APItask.Core.DTOs.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new();
        public UserResponseDto? User { get; set; }
    }

    public class OrderItemResponse
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
    }
}

