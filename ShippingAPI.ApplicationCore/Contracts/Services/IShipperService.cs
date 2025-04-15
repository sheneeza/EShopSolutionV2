using ShippingAPI.ApplicationCore.Entities;

namespace ShippingAPI.ApplicationCore.Contracts.Services;

public interface IShipperService
{
    Task<IEnumerable<Shipper>> GetAllShippersAsync();
    Task<Shipper?> GetShipperByIdAsync(int id);
    Task<int> CreateShipperAsync(Shipper shipper);
    Task<int> UpdateShipperAsync(Shipper shipper);
    Task<int> DeleteShipperAsync(int id);
    Task<IEnumerable<Shipper>> GetShippersByRegionAsync(int regionId);
}