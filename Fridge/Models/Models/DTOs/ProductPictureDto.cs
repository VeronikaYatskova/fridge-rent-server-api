﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.DTOs
{
    public class ProductPictureDto
    {
        [Required(ErrorMessage = "ProductId is a required field.")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "ImageName is a required field.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }
    }
}
