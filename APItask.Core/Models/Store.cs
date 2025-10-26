using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Core.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? StoreName { get; set; }

        [Required]
        [MaxLength(200)]
        public string? StoreAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string? StoreCity { get; set; }

        [Required]
        [MaxLength(50)]
        public string? StoreState { get; set; }

        [Required]
        [MaxLength(50)]
        public string? StoreCountry { get; set; }

        [Required]
        [MaxLength(15)]
        public string? StoreTelephone { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? StoreEmail { get; set; }

        [Required]
        [MaxLength(100)]
        public string? StoreContactName { get; set; }

        [Required]
        [MaxLength(15)]
        public string? StoreContactPhone { get; set; }
    }
}