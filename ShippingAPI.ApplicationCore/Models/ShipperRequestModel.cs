using System.ComponentModel.DataAnnotations;

namespace ShippingAPI.ApplicationCore.Models;

public class ShipperRequestModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Shipper name is required.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 256 characters.")]
    public string Name { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Shipper name is required.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 256 characters.")]
    public string ContactPerson { get; set; }
}