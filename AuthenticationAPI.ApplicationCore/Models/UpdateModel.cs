using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.ApplicationCore.Models;

public class UpdateModel
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100, MinimumLength = 8)]
    public string? Password { get; set; }
}
