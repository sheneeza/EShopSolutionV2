using PromotionsAPI.ApplicationCore.Entities;

namespace PromotionsAPI.ApplicationCore.Contracts.Services;

public interface IPromotionService
{
    Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    Task<Promotion?> GetPromotionByIdAsync(int id);
    Task<Promotion?> GetPromotionWithDetailsByIdAsync(int id);
    Task<int> CreatePromotionAsync(Promotion promotion);
    Task<int> UpdatePromotionAsync(Promotion promotion);
    Task<int> DeletePromotionAsync(int id);
    Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
    Task<IEnumerable<Promotion>> GetPromotionsByProductNameAsync(string productName);
}