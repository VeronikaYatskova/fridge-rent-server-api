using Fridge.Data.Models.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Fridge.Data.Models
{
    public class Renter : IUser
    {
        [Key]
        [Required(ErrorMessage = "Id field is required.")]
        public Guid Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = UserRoles.Renter;
    }
}
