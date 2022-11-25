using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DTOs
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
