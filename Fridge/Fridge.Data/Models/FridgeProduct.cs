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
        public Guid FridgeId { get; set; }
        public Fridge Fridge { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
