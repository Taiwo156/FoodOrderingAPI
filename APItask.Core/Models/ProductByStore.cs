using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Core.Models
{
    public class ProductByStore
    {
        [Key]
        public int ProductId { get; set; }
        [Key]
        public int StoreId { get; set; }
        public int QuantityAvailable { get; set; }
        public int QuantityCommitted { get; set; }

        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}
