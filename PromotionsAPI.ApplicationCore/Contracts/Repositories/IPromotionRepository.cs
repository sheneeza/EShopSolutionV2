using PromotionsAPI.ApplicationCore.Entities;

namespace PromotionsAPI.ApplicationCore.Contracts.Repositories;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
    Task<Promotion?> GetPromotionWithDetailsByIdAsync(int id);
    Task<IEnumerable<Promotion>> GetPromotionsByProductNameAsync(string productName);
}