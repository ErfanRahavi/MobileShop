using System.ComponentModel.DataAnnotations.Schema;

namespace MobileShop.Models
{
    public class Mobile
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } 
        public string? Specifications { get; set; }  

        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public string? ImageUrl { get; set; } 
    }
}
