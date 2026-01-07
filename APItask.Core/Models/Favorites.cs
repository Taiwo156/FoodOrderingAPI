using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APItask.Core.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int ProductId { get; set; } 
        public int UserId { get; set; } 
    }
}
