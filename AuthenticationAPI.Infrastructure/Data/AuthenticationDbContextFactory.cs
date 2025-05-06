using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthenticationAPI.Infrastructure.Data;

public class AuthenticationDbContextFactory: IDesignTimeDbContextFactory<AuthenticationDbContext>
{
    public AuthenticationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthenticationDbContext>();
        optionsBuilder.UseSqlServer(
            "Data Source=localhost;Initial Catalog=AuthenticationDatabase;User ID=sa;Password=fullStack!0; TrustServerCertificate = true");
        return new AuthenticationDbContext(optionsBuilder.Options);
    }
}