using System.ComponentModel.DataAnnotations;

namespace APItask.Core.Models
{
    public class ProductOwner
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string OwnerADObjectID { get; set; }
        [MaxLength(1000)]
        public string OwnerName { get; set; }

    }
}
