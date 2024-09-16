namespace MobileShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int MobileId { get; set; } 
        public int Quantity { get; set; } 
        public DateTime OrderDate { get; set; } 

        
        public Mobile? Mobile { get; set; }
    }
}
