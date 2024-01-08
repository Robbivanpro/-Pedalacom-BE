using PedalacomOfficial.Models;

namespace Pedalacom.Authentication.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public int? AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
