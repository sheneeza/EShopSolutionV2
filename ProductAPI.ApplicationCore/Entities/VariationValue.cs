namespace ProductAPI.ApplicationCore.Entities;

public class VariationValue
{
    public int Id { get; set; }
    public int VariationId { get; set; }
    public string Value { get; set; }

    // Navigation property
    public CategoryVariation CategoryVariation { get; set; }
    public ICollection<ProductVariationValue> ProductVariationValues { get; set; } = new List<ProductVariationValue>();
}