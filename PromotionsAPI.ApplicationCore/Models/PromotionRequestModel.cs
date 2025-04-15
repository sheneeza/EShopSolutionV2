using System.ComponentModel.DataAnnotations;

namespace PromotionsAPI.ApplicationCore.Models;

public class PromotionRequestModel
{
    public int Id { get; set; }  
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 256 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(5000, MinimumLength = 0, ErrorMessage = "Description must be between 0 and 5000 characters.")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Amount is required.")]
    [Range(typeof(decimal), "0.01", "100", ErrorMessage = "Discount must be between 0.01 and 100.")]
    public decimal Discount { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    public List<PromotionDetailRequestModel>? PromotionDetails { get; set; }
}