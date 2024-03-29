﻿using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.Requests
{
    public class AddFridgeModel
    {
        [Required(ErrorMessage = "FridgeId is a required field.")]
        public Guid FridgeId { get; set; }

        [Required(ErrorMessage = "ModelId is a required field.")]
        public Guid ModelId { get; set; }
        
        [Required(ErrorMessage = "OwnerId is a required field.")]
        public Guid OwnerId { get; set; }

        [Required(ErrorMessage = "ProducerId is a required field.")]
        public Guid ProducerId { get; set; }
        
        [Required(ErrorMessage = "Capacity is a required field.")]
        [Range(10, 1000, ErrorMessage = "Capacity can't be less than 10 and grater than 1000.")]
        public int Capacity { get; set; }
    }
}
