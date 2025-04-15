namespace PromotionsAPI.ApplicationCore.Models;

public class PromotionDetailRequestModel
{
    public int ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; } = string.Empty;
}