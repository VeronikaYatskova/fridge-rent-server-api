using Fridge.Data.Context.Configurations;
using Fridge.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Data.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.Model).WithMany(m => m.Fridges).HasForeignKey(f => f.ModelId);
            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.Producer).WithMany(p => p.Fridges).HasForeignKey(f => f.ProducerId);
            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.Owner).WithMany(o => o.Fridges).HasForeignKey(f => f.OwnerId);
            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.Renter).WithMany(r => r.Fridges).HasForeignKey(f => f.RenterId);
            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.RentDocument).WithOne(r => r.Fridge).HasForeignKey<RentDocument>(f => f.FridgeId);

            modelBuilder.Entity<RentDocument>().HasOne(f => f.Renter).WithMany(r => r.RentDocuments).HasForeignKey(f => f.RenterId);

            modelBuilder.Entity<FridgeProduct>().HasKey(fp => new { fp.FridgeId, fp.ProductId });

            modelBuilder.Entity<ProductPicture>().HasOne(pp => pp.Renter).WithMany(r => r.ProductPictures).HasForeignKey(pp => pp.RenterId);
            modelBuilder.Entity<ProductPicture>().HasOne(pp => pp.Product).WithMany(p => p.ProductPictures).HasForeignKey(pp => pp.ProductId);
        }

        public DbSet<Models.Fridge> Fridges { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Producer> Producer { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<FridgeProduct> FridgeProducts { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        public DbSet<Renter> Renters { get; set; }

        public DbSet<RentDocument> RentDocuments { get; set; }
    }
}
