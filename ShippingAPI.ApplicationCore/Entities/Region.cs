namespace ShippingAPI.ApplicationCore.Entities;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation Property
    public ICollection<ShipperRegion> ShipperRegions { get; set; } = new List<ShipperRegion>();
}