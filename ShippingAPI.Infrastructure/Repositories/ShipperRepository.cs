using Microsoft.EntityFrameworkCore;
using ShippingAPI.ApplicationCore.Contracts.Repositories;
using ShippingAPI.ApplicationCore.Entities;
using ShippingAPI.Infrastructure.Data;

namespace ShippingAPI.Infrastructure.Repositories;

public class ShipperRepository : BaseRepository<Shipper>, IShipperRepository
{
    private readonly ShippingDbContext _dbContext;

    public ShipperRepository(ShippingDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Shipper>> GetShippersByRegionAsync(int regionId)
    {
        // Returns shippers that have a ShipperRegion entry matching regionId
        return await _dbContext.Shippers
            .Where(s => s.ShipperRegions.Any(sr => sr.RegionId == regionId))
            .ToListAsync();
    }
}