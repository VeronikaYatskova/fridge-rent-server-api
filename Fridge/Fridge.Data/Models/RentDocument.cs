using System.ComponentModel.DataAnnotations;

namespace Fridge.Data.Models
{
    public class RentDocument
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "StartDate is a requird field.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is a requird field.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "MonthCost is a requird field.")]
        [Range(10, 70)]
        public decimal MonthCost { get; set; }


        [Required]
        public Guid RenterId { get; set; }
        public Renter Renter { get; set; }

        [Required]
        public Guid FridgeId { get; set; }
        public Fridge Fridge { get; set; }
    }
}
