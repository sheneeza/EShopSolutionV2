namespace ShippingAPI.ApplicationCore.Entities;

public class ShippingDetails
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ShipperId { get; set; }
    public string ShippingStatus { get; set; } = default!;
    public string? TrackingNumber { get; set; }

    // Navigation Property
    public Shipper? Shipper { get; set; }
}