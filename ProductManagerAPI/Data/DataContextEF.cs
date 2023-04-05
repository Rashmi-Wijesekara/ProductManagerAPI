using Microsoft.EntityFrameworkCore;
using ProductManagerAPI.Models;

namespace ProductManagerAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ProductsSchema");

            modelBuilder.Entity<Product>()
                .ToTable("Product", "ProductsSchema")
                .HasKey(product => product.ProdId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }
    }
}
