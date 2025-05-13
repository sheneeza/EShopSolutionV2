namespace ShippingAPI.ApplicationCore.Models;

public class ShippingStatusRequestModel
{
    public int    OrderId { get; set; }
    public string Status  { get; set; }
}