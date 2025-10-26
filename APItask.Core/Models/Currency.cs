namespace APItask.Core.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "USD"
        public string Symbol { get; set; } = string.Empty; // e.g., "$"
        // Add other currency properties as needed
    }
}