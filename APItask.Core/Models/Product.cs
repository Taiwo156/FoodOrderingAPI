using System;
using System.ComponentModel.DataAnnotations;

namespace APItask.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MinLength(5), MaxLength(1000)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(5), MaxLength(8000)]
        public string Descriptions { get; set; } = string.Empty;

        [Range(5, 9000)]
        public decimal Price { get; set; }

        public DateTime AvailableSince { get; set; }
        public DateTime CreatedDate { get; set; }

        [MaxLength(200)]
        public string? CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [MaxLength(200)]
        public string? ModifiedBy { get; set; }

        public bool IsActive { get; set; }
        public string? UPC { get; set; }
    }

    public class ProductImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public string Mime { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
