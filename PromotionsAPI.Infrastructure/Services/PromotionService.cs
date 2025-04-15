using PromotionsAPI.ApplicationCore.Contracts.Repositories;
using PromotionsAPI.ApplicationCore.Contracts.Services;
using PromotionsAPI.ApplicationCore.Entities;

namespace PromotionsAPI.Infrastructure.Services;

public class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository;
        
    public PromotionService(IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync() =>
        await _promotionRepository.GetAllAsync();

    public async Task<Promotion?> GetPromotionByIdAsync(int id) =>
        await _promotionRepository.GetByIdAsync(id);

    public async Task<Promotion?> GetPromotionWithDetailsByIdAsync(int id) =>
        await _promotionRepository.GetPromotionWithDetailsByIdAsync(id);

    public async Task<int> CreatePromotionAsync(Promotion promotion) =>
        await _promotionRepository.InsertAsync(promotion);

    public async Task<int> UpdatePromotionAsync(Promotion promotion) =>
        await _promotionRepository.UpdateAsync(promotion);

    public async Task<int> DeletePromotionAsync(int id) =>
        await _promotionRepository.DeleteAsync(id);

    public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync() =>
        await _promotionRepository.GetActivePromotionsAsync();
    
    public async Task<IEnumerable<Promotion>> GetPromotionsByProductNameAsync(string productName)
    {
        return await _promotionRepository.GetPromotionsByProductNameAsync(productName);
    }

}