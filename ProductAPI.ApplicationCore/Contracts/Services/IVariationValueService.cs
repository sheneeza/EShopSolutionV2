using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Services;

public interface IVariationValueService
{
    Task<int> InsertAsync(VariationValue variationValue);
    Task<int> UpdateAsync(VariationValue variationValue);
    Task<int> DeleteAsync(int id);
        
    Task<IEnumerable<VariationValue>> GetAllAsync();
    Task<VariationValue?> GetByIdAsync(int id);
    
    Task<IEnumerable<VariationValue>> GetByVariationIdAsync(int variationId);
}