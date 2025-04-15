namespace ProductAPI.ApplicationCore.Entities;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public string ProductImage { get; set; }
    public string Sku { get; set; }
    
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ProductCategory ProductCategory { get; set; }
    public ICollection<ProductVariationValue> ProductVariationValues { get; set; } = new List<ProductVariationValue>();
}