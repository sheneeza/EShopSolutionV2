namespace ShippingAPI.ApplicationCore.Entities;

public class Shipper
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? EmailId { get; set; }
    public string? Phone { get; set; }
    public string? ContactPerson { get; set; }

    // Navigation Properties
    public ICollection<ShipperRegion> ShipperRegions { get; set; } = new List<ShipperRegion>();
    public ICollection<ShippingDetails> ShippingDetails { get; set; } = new List<ShippingDetails>();
}