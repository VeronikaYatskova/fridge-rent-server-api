using Fridge.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Data.Context.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
            (
                new Product
                {
                    Id = new Guid("36B74198-A896-429F-B040-0512FCA189A8"),
                    Name = "Apple",
                    DefaultQuantity = 3,
                },
                new Product
                {
                    Id = new Guid("F2DDEA9C-7691-4C7A-99EC-ABAEC36DB9BD"),
                    Name = "Milk",
                    DefaultQuantity = 1,
                },
                new Product
                {
                    Id = new Guid("FDB08EB6-D113-4D8A-8576-3454BB89AD55"),
                    Name = "Eggs",
                    DefaultQuantity = 10,
                },
                new Product
                {
                    Id = new Guid("B89AA809-9FAC-4C67-AF3B-6599ADE45F92"),
                    Name = "Cake",
                    DefaultQuantity = 1,
                },
                new Product
                {
                    Id = new Guid("3AB533B7-2AE5-4121-85DD-9D977E1B53ED"),
                    Name = "Tomato",
                    DefaultQuantity = 5,
                }
            );
        }
    }
}
