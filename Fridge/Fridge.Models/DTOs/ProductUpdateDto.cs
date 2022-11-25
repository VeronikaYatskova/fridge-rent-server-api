using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs
{
    public class ProductUpdateDto
    {
        public Guid FridgeId { get; set; }

        public Guid ProductId { get; set; }
        /// <summary>
        /// Count of product that is put in the fridge by default.
        /// </summary>
        [Required(ErrorMessage = "DefaultQuantity is a required field.")]
        [Range(1, int.MaxValue, ErrorMessage = "DefaultQuantity is required and it can't be less than 10")]
        public int Count { get; set; }
    }
}
