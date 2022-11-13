using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Models
{
    public class FridgeProduct
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Count of products are put in the fridge.
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// Fridge identifier.
        /// </summary>
        [ForeignKey(nameof(Fridge))]
        public Guid FridgeId { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
    }
}
