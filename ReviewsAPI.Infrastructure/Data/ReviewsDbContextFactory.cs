using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ReviewsAPI.Infrastructure.Data;

public class ReviewsDbContextFactory : IDesignTimeDbContextFactory<ReviewsDbContext>
{
    public ReviewsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReviewsDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ReviewsDatabase;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");

        return new ReviewsDbContext(optionsBuilder.Options);
    }
}