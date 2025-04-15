using System.ComponentModel.DataAnnotations;

namespace ReviewsAPI.ApplicationCore.Models;

public class CustomerReviewRequestModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    
    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 256 characters.")]
    public string CustomerName { get; set; } = string.Empty;
    public int OrderId { get; set; }
    
    [Required(ErrorMessage = "Order date is required.")]
    public DateTime OrderDate { get; set; }
    public int ProductId { get; set; }
    
    [Required(ErrorMessage = "Product name is required.")]
    public string ProductName { get; set; } = string.Empty;
    public int RatingValue { get; set; }
    
    [Required(ErrorMessage = "Comment is required.")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Comment must be between 2 and 500 characters.")]
    public string? Comment { get; set; }
    
    [Required(ErrorMessage = "Review date is required.")]
    public DateTime ReviewDate { get; set; }
}