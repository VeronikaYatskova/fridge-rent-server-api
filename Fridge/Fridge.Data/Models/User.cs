using System.ComponentModel.DataAnnotations;


namespace Fridge.Data.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Id field is required.")]
        public Guid Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }


        public virtual ICollection<Fridge> RenterFridges { get; set; }
        public virtual ICollection<Fridge> OwnerFridges { get; set; }

        public virtual ICollection<ProductPicture> ProductPictures { get; set; } 
    }
}
