using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.Requests
{
    public class AddFridgeOwnerModel
    {
        [Required(ErrorMessage = "ModelId is a required field.")]
        public string ModelId { get; set; }

        [Required(ErrorMessage = "ProducerId is a required field.")]
        public string ProducerId { get; set; }

        [Required(ErrorMessage = "Capacity is a required field.")]
        [Range(10, 1000, ErrorMessage = "Capacity can't be less than 10 and grater than 1000.")]
        public int Capacity { get; set; }
    }
}
