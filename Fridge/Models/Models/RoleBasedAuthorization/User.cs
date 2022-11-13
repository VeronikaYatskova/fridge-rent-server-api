using System.ComponentModel.DataAnnotations;
using Models.Models.RoleBasedAuthorization;

namespace Fridge.Models.RoleBasedAuthorization
{
    public class User : IUser
    {
        [Required(ErrorMessage = "Id field is required.")]
        public Guid Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = UserRoles.Renter;

        public string Token { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Expires { get; set; }
    }
}
