using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class Model
    {
        /// <summary>
        /// Model identifier.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "Id is a required field.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Model name.
        /// </summary>
        [Column("ModelName")]
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string Name { get; set; }

        public ICollection<Models.Fridge> Fridges { get; set; }
    }
}