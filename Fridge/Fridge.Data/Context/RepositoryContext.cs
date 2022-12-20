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
            modelBuilder.Entity<Models.Fridge>().HasOne(f => f.Renter).WithMany(r => r.Fridges).HasForeignKey(f => f.RenterId);
            
            modelBuilder.Entity<FridgeProduct>().HasKey(fp => new { fp.FridgeId, fp.ProductId });
            
            modelBuilder.Entity<ProductPicture>().HasOne(pp => pp.Renter).WithMany(r => r.ProductPictures).HasForeignKey(pp => pp.RenterId);
            modelBuilder.Entity<ProductPicture>().HasOne(pp => pp.Product).WithMany(p => p.ProductPictures).HasForeignKey(pp => pp.ProductId);
        }

        public DbSet<Models.Fridge> Fridges { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Producer> Producer { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<FridgeProduct> FridgeProducts { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
