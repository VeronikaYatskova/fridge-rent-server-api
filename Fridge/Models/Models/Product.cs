using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Models
{
    public class Product
    {
        /// <summary>
        /// Product identifier.
        /// </summary>
        [Column("ProductId")]
        public Guid Id { get; set; }

        
        /// <summary>
        /// Product name.
        /// </summary>
        [Column("ProductName")]
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Count of product that is put in the fridge by default.
        /// </summary>
        [Required(ErrorMessage = "DefaultQuantity is a required field.")]
        [Range(10, int.MaxValue, ErrorMessage = "DefaultQuantity is required and it can't be less than 10")]
        public int DefaultQuantity { get; set; }
    }
}
