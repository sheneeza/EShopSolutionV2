using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShippingAPI.Infrastructure.Data;

public class ShippingDbContextFactory : IDesignTimeDbContextFactory<ShippingDbContext>
{
    public ShippingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShippingDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ShippingDatabase;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");

        return new ShippingDbContext(optionsBuilder.Options);
    }
}