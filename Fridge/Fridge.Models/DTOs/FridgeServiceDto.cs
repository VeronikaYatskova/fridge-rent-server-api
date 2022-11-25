namespace Fridge.Models.DTOs
{
    public class FridgeServiceDto
    {
        public Guid FridgeId { get; set; }

        public Guid ModelId { get; set; }

        public Guid OwnerId { get; set; }

        public Guid ProducerId { get; set; }

        public int Capacity { get; set; }
    }
}
