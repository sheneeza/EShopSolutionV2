namespace PromotionsAPI.ApplicationCore.Entities;

public class PromotionDetail
{
    public int Id { get; set; }
    
    public int PromotionId { get; set; }
    
    public int ProductCategoryId { get; set; }
    
    public string ProductCategoryName { get; set; } = string.Empty;
    
    public Promotion Promotion { get; set; }
}