using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductAPI.Infrastructure.Data;

namespace OrderAPI.Infrastructure.Data;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ProductDatabase;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");

        return new ProductDbContext(optionsBuilder.Options);
    }
}