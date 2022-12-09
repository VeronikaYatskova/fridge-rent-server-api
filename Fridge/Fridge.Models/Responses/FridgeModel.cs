using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.Responses
{
    public class FridgeModel
    {
        public Guid Id { get; set; }

        public string Model { get; set; }

        public string Owner { get; set; }

        public string Producer { get; set; }

        public string? Renter { get; set; }

        public int Capacity { get; set; }

        public int CurrentCount { get; set; } = 0;
    }
}
