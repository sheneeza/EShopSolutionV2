using ShippingAPI.ApplicationCore.Contracts.Repositories;
using ShippingAPI.ApplicationCore.Contracts.Services;
using ShippingAPI.ApplicationCore.Entities;

namespace ShippingAPI.Infrastructure.Services;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _shipperRepository;

    public ShipperService(IShipperRepository shipperRepository)
    {
        _shipperRepository = shipperRepository;
    }

    public async Task<IEnumerable<Shipper>> GetAllShippersAsync()
    {
        return await _shipperRepository.GetAllAsync();
    }

    public async Task<Shipper?> GetShipperByIdAsync(int id)
    {
        return await _shipperRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateShipperAsync(Shipper shipper)
    {
        // check for user and admin here
        return await _shipperRepository.InsertAsync(shipper);
    }

    public async Task<int> UpdateShipperAsync(Shipper shipper)
    {
        // check for user and admin here
        return await _shipperRepository.UpdateAsync(shipper);
    }

    public async Task<int> DeleteShipperAsync(int id)
    {
        return await _shipperRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Shipper>> GetShippersByRegionAsync(int regionId)
    {
        return await _shipperRepository.GetShippersByRegionAsync(regionId);
    }
}