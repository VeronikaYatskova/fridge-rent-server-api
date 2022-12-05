using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class ProductPicture
    {
        [Key]
        [Required(ErrorMessage = "Id is a required field.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ProductId is a required field.")]
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "UserId is a required field.")]
        [ForeignKey(nameof(Renter))]
        public Guid RenterId { get; set; }

        [Required(ErrorMessage = "ImageName is a required field.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "ImagePath is a required field.")]
        public string ImagePath { get; set; }
    }
}
