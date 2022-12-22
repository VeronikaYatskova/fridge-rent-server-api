namespace Fridge.Models.Responses
{
    public interface IUserFridgeModel
    {
        public Guid Id { get; set; }

        public string Model { get; set; }

        public string Owner { get; set; }

        public string Producer { get; set; }

        public int Capacity { get; set; }
    }
}
