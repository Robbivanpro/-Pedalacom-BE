using System.ComponentModel.DataAnnotations;

namespace PedalacomOfficial.Models
{
        public class CartItem
        {
            [Key]
            public int CartId { get; set; }
            public int CustomerId { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public virtual Customer Customer { get; set; }
            public virtual Product Product { get; set; }
        }
    
}
