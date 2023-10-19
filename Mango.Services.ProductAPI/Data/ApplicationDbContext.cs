using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                new Product() 
                {
                    ProductId = 1,
                    Name = "Product 1",
                    Price = 50,
                    Description = "Description 1",
                    ImageUrl = "https://placehold.co/603*403",
                    CategoryName = "category 1"
                },
                new Product()
                {
                    ProductId = 2,
                    Price = 30,
                    Name = "Product 2",
                    Description = "Description 2",
                    ImageUrl = "https://placehold.co/603*403",
                    CategoryName = "category 2"
                },
                new Product()
                {
                    ProductId = 3,
                    Name = "Product 2",
                    Price = 40,
                    Description = "Description 2",
                    ImageUrl = "https://placehold.co/603*403",
                    CategoryName = "category 2"
                }

             );
            base.OnModelCreating(modelBuilder);

        }
    }
}
