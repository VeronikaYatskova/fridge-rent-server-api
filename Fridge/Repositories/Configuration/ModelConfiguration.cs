using Fridge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fridge.Configuration
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasData
            (
                new Model
                {
                    Id = new Guid("F8A3B786-D4B2-49D7-953B-578729B55A35"),
                    Name = "Indesit ITR 5200 W",
                },
                new Model
                {
                    Id = new Guid("4A645006-5621-4536-9490-E1769FAC2F53"),
                    Name = "LG GA-B379SLUL",
                },
                new Model
                {
                    Id = new Guid("44DC042A-3453-4C17-A4D1-CD8C0AC9378C"),
                    Name = "ATLANT XM-4208-000",
                },
                new Model
                {
                    Id = new Guid("2182354C-D8CC-47BF-844F-4AAFABA1DBFE"),
                    Name = "ATLANT ХМ 4625-101 NL",
                },
                new Model 
                {
                    Id = new Guid("AF96137E-0B17-41B5-A819-A5A23DA0FD97"),
                    Name = "Toshiba GR-RF610WE-PMS",
                }
            );
        }
    }
}
