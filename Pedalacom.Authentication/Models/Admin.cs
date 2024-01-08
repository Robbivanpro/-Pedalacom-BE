using System.ComponentModel.DataAnnotations;

namespace Pedalacom.Authentication.Models
{
    public class Admin
    {
        [Key]
        public int? AdminId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
       
    }
}
