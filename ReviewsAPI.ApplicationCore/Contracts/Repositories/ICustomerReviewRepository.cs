using ReviewsAPI.ApplicationCore.Entities;

namespace ReviewsAPI.ApplicationCore.Contracts.Repositories;

public interface ICustomerReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId);
    Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId);
}