using System.ComponentModel.DataAnnotations;

namespace Fridge.Models.Responses
{
    public class ProductWithCurrentCountAndNameModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
