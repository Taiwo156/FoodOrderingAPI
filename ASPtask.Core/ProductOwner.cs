using System.ComponentModel.DataAnnotations;

namespace APItask
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
