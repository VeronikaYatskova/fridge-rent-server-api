using System.ComponentModel.DataAnnotations;


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
        public virtual Fridge Fridge { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
