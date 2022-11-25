using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs.FridgeProductDtos
{
    public class FridgeProductDto
    {
        [Required(ErrorMessage = "FridgeId is a required field.")]
        public Guid FridgeId { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "ProductId is a required field.")]
        public Guid ProductId { get; set; } = Guid.Empty;

        [Range(0, 20)]
        [Required(ErrorMessage = "Count is a required field.")]
        public int Count { get; set; } = 0;
    }
}
