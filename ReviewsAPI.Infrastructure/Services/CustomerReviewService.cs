using ReviewsAPI.ApplicationCore.Contracts.Repositories;
using ReviewsAPI.ApplicationCore.Contracts.Services;
using ReviewsAPI.ApplicationCore.Entities;

namespace ReviewsAPI.Infrastructure.Services;

public class CustomerReviewService : ICustomerReviewService
{
    private readonly ICustomerReviewRepository _repository;

    public CustomerReviewService(ICustomerReviewRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Review?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<int> CreateAsync(Review review)
        => await _repository.InsertAsync(review);

    public async Task<int> UpdateAsync(Review review)
        => await _repository.UpdateAsync(review);

    public async Task<int> DeleteAsync(int id)
        => await _repository.DeleteAsync(id);

    public async Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId)
        => await _repository.GetReviewsByUserAsync(userId);

    public async Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId)
        => await _repository.GetReviewsByProductAsync(productId);

    public async Task<bool> ApproveReviewAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Status = "Approved";
        var result = await _repository.UpdateAsync(existing);
        return result > 0;
    }

    public async Task<bool> RejectReviewAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Status = "Rejected";
        var result = await _repository.UpdateAsync(existing);
        return result > 0;
    }
}