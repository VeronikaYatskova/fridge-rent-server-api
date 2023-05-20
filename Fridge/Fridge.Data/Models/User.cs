using System.ComponentModel.DataAnnotations;


namespace Fridge.Data.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Id field is required.")]
        public Guid Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string Role { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? TokenCreated { get; set; }

        public DateTime? TokenExpires { get; set; }

        public string? SocialId { get; set; }

        public string? AuthVia { get; set; }


        public virtual ICollection<Fridge> RenterFridges { get; set; }
        public virtual ICollection<Fridge> OwnerFridges { get; set; }
        public virtual ICollection<ProductPicture> ProductPictures { get; set; } 
    }
}
