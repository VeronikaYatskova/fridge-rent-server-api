
namespace Fridge.Models.DTOs.FridgeDtos
{
    public class RentDocumentDto
    {
        public Guid Id { get; set; }

        public string RenterEmail { get; set; }

        public string OwnerName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal MonthCost { get; set; }
    }
}
