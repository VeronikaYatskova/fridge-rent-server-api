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
        }

        public DbSet<Models.Fridge> Fridges { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Producer> Producer { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<FridgeProduct> FridgeProducts { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserFridge> UserFridges { get; set; }

        public DbSet<RentDocument> RentDocuments { get; set; }
    }
}
