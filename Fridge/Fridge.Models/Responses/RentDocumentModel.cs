namespace Fridge.Models.Responses
{
    public class RentDocumentModel
    {
        public Guid Id { get; set; }

        public string RenterEmail { get; set; }

        public string OwnerName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal MonthCost { get; set; }
    }
}
