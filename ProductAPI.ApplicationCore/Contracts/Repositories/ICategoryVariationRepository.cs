using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Repositories;

public interface ICategoryVariationRepository : IRepository<CategoryVariation>
{
    Task<IEnumerable<CategoryVariation>> GetByCategoryIdAsync(int categoryId);
}