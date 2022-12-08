using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs.FridgeDtos
{
    public class FridgeDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Model is a required field.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Owner is a required field.")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Producer is a required field.")]
        public string Producer { get; set; }

        public string? Renter { get; set; }

        [Required(ErrorMessage = "Capacity is a required field.")]
        [Range(10, int.MaxValue, ErrorMessage = "Capacity is required and it can't be less than 10")]
        public int Capacity { get; set; }

        public int CurrentCount { get; set; } = 0;
    }
}
