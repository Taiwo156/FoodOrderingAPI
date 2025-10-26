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
        public int ProductId { get; set; }
        public Product Product { get; set; }  // Reference to Product table

        public int StoreId { get; set; }
        public Store Store { get; set; }      // Reference to Store table

        public int QuantityAvailable { get; set; }
        public int QuantityCommitted { get; set; }
    }
}
