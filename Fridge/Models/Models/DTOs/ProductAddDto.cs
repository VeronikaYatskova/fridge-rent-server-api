using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DTOs
{
    public class ProductAddDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
