namespace ProductAPI.ApplicationCore.Entities;

public class ProductVariationValue
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int VariationValueId { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public VariationValue VariationValue { get; set; }
}