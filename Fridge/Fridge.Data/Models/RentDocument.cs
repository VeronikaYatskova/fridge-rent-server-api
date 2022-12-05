using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Data.Models
{
    public class RentDocument
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "FridgeId is a requird field.")]
        [ForeignKey(nameof(Models.Fridge))]
        public Guid FridgeId { get; set; }

        [Required(ErrorMessage = "UserId is a requird field.")]
        [ForeignKey(nameof(Renter))]
        public Guid RenterId { get; set; }

        [Required(ErrorMessage = "StartDate is a requird field.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is a requird field.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "MonthCost is a requird field.")]
        [Range(10, 70)]
        public decimal MonthCost { get; set; }
    }
}
