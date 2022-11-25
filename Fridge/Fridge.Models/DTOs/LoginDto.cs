﻿using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs
{
    public class LoginDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is a required field.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is a required field.")]
        public string Password { get; set; } = string.Empty;
    }
}