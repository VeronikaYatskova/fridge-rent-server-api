using Fridge.Models.RoleBasedAuthorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fridge.Models
{
    public class UserFridge
    {
        [Required(ErrorMessage = "Id field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "UserId field is required.")]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "FridgeId field is required.")]
        [ForeignKey(nameof(Fridge))]
        public Guid FridgeId { get; set; }

        [Required(ErrorMessage = "RentDocumentId field is required.")]
        [ForeignKey(nameof(RentDocument))]
        public Guid RentDocumentId { get; set; }
        public RentDocument RentDocument { get; set; }
    }
}
