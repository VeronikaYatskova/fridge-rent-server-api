using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class Producer
    {
        /// <summary>
        /// Producer identifier.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "Id is a required field.")]
        [Column("ProducerId")]
        public Guid Id { get; set; }

        /// <summary>
        /// Producer name.
        /// </summary>
        [Column("ProducerName")]
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Producer country.
        /// </summary>
        [Column("ProducerCountry")]
        [Required(ErrorMessage = "Country is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Country is 60 characters")]
        public string Country { get; set; }

        public virtual ICollection<Models.Fridge> Fridges { get; set; }
    }
}
