using Microsoft.EntityFrameworkCore;
using ReviewsAPI.ApplicationCore.Contracts.Repositories;
using ReviewsAPI.ApplicationCore.Entities;
using ReviewsAPI.Infrastructure.Data;

namespace ReviewsAPI.Infrastructure.Repositories;

public class CustomerReviewRepository 
    : BaseRepository<Review>, ICustomerReviewRepository
{
    private readonly ReviewsDbContext _context;

    public CustomerReviewRepository(ReviewsDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId)
    {
        return await _dbSet
            .Where(r => r.CustomerId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId)
    {
        return await _dbSet
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }
}