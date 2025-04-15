namespace PromotionsAPI.ApplicationCore.Entities;

public class Promotion
{
   
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; }
    
    public decimal Discount { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    
    public ICollection<PromotionDetail> PromotionDetails { get; set; } 
        = new List<PromotionDetail>();
}