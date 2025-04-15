using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderAPI.Infrastructure.Data;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EShopVAApr2024;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");

        return new OrderDbContext(optionsBuilder.Options);
    }
}