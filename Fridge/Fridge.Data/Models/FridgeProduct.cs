using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class FridgeProduct
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Count of products are put in the fridge.
        /// </summary>
        [Required]
        public int Count { get; set; } = 0;

        /// <summary>
        /// Fridge identifier.
        /// </summary>
        [Required]
        [ForeignKey(nameof(Fridge))]
        public Guid FridgeId { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [Required]
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
    }
}
