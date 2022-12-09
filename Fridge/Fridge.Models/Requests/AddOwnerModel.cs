using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.Requests
{
    public class AddOwnerModel
    {
        /// <summary>
        /// Owner's name.
        /// </summary>
        [Required(ErrorMessage = "Name is a required field.")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is a required field.")]
        public string Password { get; set; }

        /// <summary>
        /// Owner's phone.
        /// </summary>
        [Required(ErrorMessage = "Phone is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Phone is 60 characters")]
        public string Phone { get; set; }
    }
}
