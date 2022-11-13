using Fridge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Configuration
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.HasData
            (
                new Producer
                {
                    Id = new Guid("D347DFE3-5CF9-49E8-8137-8880580F203B"),
                    Name = "ATLANT",
                    Country = "Belarus"
                },
                new Producer
                {
                    Id = new Guid("A8000178-A46B-4122-8758-2931E99C46E9"),
                    Name = "Indesit",
                    Country = "Russia"
                },
                new Producer
                {
                    Id = new Guid("38886C70-4593-47CE-9CD1-99D9831C2EB4"),
                    Name = "LG",
                    Country = "Russia"
                },
                new Producer
                {
                    Id = new Guid("0D08C561-361C-497E-BD21-06A7CE7D5516"),
                    Name = "Toshiba",
                    Country = "China"
                },
                new Producer
                {
                    Id = new Guid("8E652090-8FA2-4271-8E05-7934A0BA77A7"),
                    Name = "BEKO",
                    Country = "Russia"
                }
            );
        }
    }
}
