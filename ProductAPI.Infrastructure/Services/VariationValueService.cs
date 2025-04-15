using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public class VariationValueService : IVariationValueService
{
    private readonly IVariationValueRepository _repository;

    public VariationValueService(IVariationValueRepository repository)
    {
        _repository = repository;
    }

    public Task<int> InsertAsync(VariationValue variationValue)
    {
        return _repository.InsertAsync(variationValue);
    }

    public Task<int> UpdateAsync(VariationValue variationValue)
    {
        return _repository.UpdateAsync(variationValue);
    }

    public Task<int> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    public Task<IEnumerable<VariationValue>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<VariationValue?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<IEnumerable<VariationValue>> GetByVariationIdAsync(int variationId)
    {
        return _repository.GetByVariationIdAsync(variationId);
    }
}