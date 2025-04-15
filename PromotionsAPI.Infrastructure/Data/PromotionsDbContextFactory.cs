using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PromotionsAPI.Infrastructure.Data;

public class PromotionsDbContextFactory : IDesignTimeDbContextFactory<PromotionsDbContext>
{
    public PromotionsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PromotionsDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PromotionsDatabase;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");

        return new PromotionsDbContext(optionsBuilder.Options);
    }
}