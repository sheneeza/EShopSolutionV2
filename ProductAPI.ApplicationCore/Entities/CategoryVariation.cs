namespace ProductAPI.ApplicationCore.Entities;

public class CategoryVariation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string VariationName { get; set; }

    // Navigation property
    public ProductCategory ProductCategory { get; set; }
    public ICollection<VariationValue> VariationValues { get; set; } = new List<VariationValue>();
}