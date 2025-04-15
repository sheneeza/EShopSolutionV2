using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Repositories;

public interface IVariationValueRepository : IRepository<VariationValue>
{
    Task<IEnumerable<VariationValue>> GetByVariationIdAsync(int variationId);
}