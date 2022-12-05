using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class Fridge
    {
        /// <summary>
        /// Fridge identifier.
        /// </summary>
        [Required]
        [Key]
        [Column("FridgeId")]
        public Guid Id { get; set; }

        public bool IsRented { get; set; } = false;

        /// <summary>
        /// Count of products the fridge can contain.
        /// </summary>
        [Required(ErrorMessage = "Capacity is a required field.")]
        [Range(10, int.MaxValue, ErrorMessage ="Capacity is required and it can't be less than 10")]
        public int Capacity { get; set; }

        /// <summary>
        /// Model identifier.
        /// </summary>
        //[ForeignKey(nameof(Model))]
        [Required(ErrorMessage = "ModelId is a required field.")]
        public Guid ModelId { get; set; }
        public Model Model { get; set; }

        /// <summary>
        /// Owner identifier.
        /// </summary>
        [ForeignKey(nameof(Owner))]
        [Required(ErrorMessage = "OwnerId is a required field.")]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Producer identifier.
        /// </summary>
        [ForeignKey(nameof(Producer))]
        [Required(ErrorMessage = "ProducerId is a required field.")]
        public Guid ProducerId { get; set; }
    }
}
