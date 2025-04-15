namespace ReviewsAPI.ApplicationCore.Entities;

public class Review
{
    
    // Primary key for the Review entity.
    public int Id { get; set; }
    
    // Foreign key referencing the customer who wrote the review.
    public int CustomerId { get; set; }
    
    // Name of the customer (duplicate data for easy reference in the review).
    public string CustomerName { get; set; } = default!;
    
    public int OrderId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public int ProductId { get; set; }
    
    public string ProductName { get; set; } = default!;
    
    public int RatingValue { get; set; }
    
    public string? Comment { get; set; }
    
    public DateTime ReviewDate { get; set; }
    
    public string Status { get; set; } = "Pending";
}