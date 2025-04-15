using Microsoft.EntityFrameworkCore;
using PromotionsAPI.ApplicationCore.Entities;

namespace PromotionsAPI.Infrastructure.Data;

public class PromotionsDbContext : DbContext
{
    public PromotionsDbContext(DbContextOptions<PromotionsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Promotion> Promotions { get; set; } 
    public DbSet<PromotionDetail> PromotionDetails { get; set; }
}