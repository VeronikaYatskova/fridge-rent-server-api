

namespace Fridge.Models.Responses
{
    public class FridgeRenterModel
    {
        public Guid Id { get; set; }

        public string Model { get; set; }

        public string Owner { get; set; }

        public string Producer { get; set; }

        public int Capacity { get; set; }

        public int CurrentCount { get; set; } = 0;
    }
}
