namespace ShippingAPI.ApplicationCore.Entities;

public class ShipperRegion
{
    public int ShipperId { get; set; }
    public int RegionId { get; set; }
    public bool Active { get; set; }

    // Navigation Properties
    public Shipper? Shipper { get; set; }
    public Region? Region { get; set; }
}