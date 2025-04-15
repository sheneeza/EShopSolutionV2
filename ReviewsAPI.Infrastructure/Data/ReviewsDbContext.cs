using Microsoft.EntityFrameworkCore;
using ReviewsAPI.ApplicationCore.Entities;

namespace ReviewsAPI.Infrastructure.Data;

public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Review> Reviews { get; set; } = null!;
}