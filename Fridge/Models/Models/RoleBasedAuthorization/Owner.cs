using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Models.RoleBasedAuthorization;

namespace Fridge.Models.RoleBasedAuthorization
{
    public class Owner : IUser
    {
        /// <summary>
        /// Owner's identifier.
        /// </summary>
        [Column("OwnerId")]
        public Guid Id { get; set; }

        /// <summary>
        /// Owner's name.
        /// </summary>
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        /// <summary>
        /// Owner's phone.
        /// </summary>
        [Required(ErrorMessage = "Phone is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Phone is 60 characters")]
        public string Phone { get; set; }

        [NotMapped]
        public string Role { get; } = UserRoles.Owner;

        public string Token { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Expires { get; set; }
    }
}
