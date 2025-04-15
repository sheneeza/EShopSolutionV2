using ShippingAPI.ApplicationCore.Entities;

namespace ShippingAPI.ApplicationCore.Contracts.Repositories;

public interface IShipperRepository : IRepository<Shipper>
{
    
    Task<IEnumerable<Shipper>> GetShippersByRegionAsync(int regionId);
}