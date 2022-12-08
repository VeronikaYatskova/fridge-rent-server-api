using System.ComponentModel.DataAnnotations;


namespace Fridge.Data.Models
{
    public class ProductPicture
    {
        [Key]
        [Required(ErrorMessage = "Id is a required field.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ProductId is a required field.")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required(ErrorMessage = "UserId is a required field.")]
        public Guid RenterId { get; set; }
        public Renter Renter { get; set; }

        [Required(ErrorMessage = "ImageName is a required field.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "ImagePath is a required field.")]
        public string ImagePath { get; set; }
    }
}
