using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace myshop.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
