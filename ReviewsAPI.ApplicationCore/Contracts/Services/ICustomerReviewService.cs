using ReviewsAPI.ApplicationCore.Entities;

namespace ReviewsAPI.ApplicationCore.Contracts.Services;

public interface ICustomerReviewService
{
    Task<IEnumerable<Review>> GetAllAsync();
    Task<Review?> GetByIdAsync(int id);
    Task<int> CreateAsync(Review review);
    Task<int> UpdateAsync(Review review);
    Task<int> DeleteAsync(int id);

    Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId);
    Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId);

    Task<bool> ApproveReviewAsync(int id);
    Task<bool> RejectReviewAsync(int id);
}