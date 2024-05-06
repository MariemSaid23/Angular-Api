using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.core.Entities;

namespace Talabat.Repositery.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //old 
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
    }
}
