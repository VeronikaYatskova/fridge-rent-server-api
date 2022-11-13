using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs
{
    public class ProductCountDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Count is a requierd field.")]
        public int Count { get; set; }
    }
}
