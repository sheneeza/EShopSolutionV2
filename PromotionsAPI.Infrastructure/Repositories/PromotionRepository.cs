using Microsoft.EntityFrameworkCore;
using PromotionsAPI.ApplicationCore.Contracts.Repositories;
using PromotionsAPI.ApplicationCore.Entities;
using PromotionsAPI.Infrastructure.Data;

namespace PromotionsAPI.Infrastructure.Repositories;

public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
{
    public PromotionRepository(PromotionsDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(p => p.StartDate <= now && p.EndDate >= now)
            .Include(p => p.PromotionDetails)
            .ToListAsync();
    }

    public async Task<Promotion?> GetPromotionWithDetailsByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.PromotionDetails)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Promotion>> GetPromotionsByProductNameAsync(string productName)
    {
        return await _dbSet
            .Where(p => p.PromotionDetails.Any(pd => pd.ProductCategoryName.Contains(productName)))
            .ToListAsync();
    }

}