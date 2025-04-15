namespace ProductAPI.ApplicationCore.Entities;

public class ProductCategory
{
    public int Id { get; set; }
        
    // Nullable if a category has no parent (top-level)
    public int? ParentCategoryId { get; set; }
    public string CategoryName { get; set; }

    // Navigation properties
    public ProductCategory ParentCategory { get; set; }
    public ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();
    public ICollection<CategoryVariation> CategoryVariations { get; set; } = new List<CategoryVariation>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}