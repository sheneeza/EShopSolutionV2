using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Data;

 public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        { }

        // DbSets for each entity
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<CategoryVariation> CategoryVariations { get; set; }
        public DbSet<VariationValue> VariationValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariationValue> ProductVariationValues { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- ProductCategory configuration ---
            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryName)
                      .IsRequired()
                      .HasMaxLength(255);

                // Self-referencing relationship: use Restrict to avoid cycles and multiple cascade paths.
                entity.HasOne(e => e.ParentCategory)
                      .WithMany(e => e.SubCategories)
                      .HasForeignKey(e => e.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- CategoryVariation configuration ---
            modelBuilder.Entity<CategoryVariation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VariationName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasOne(e => e.ProductCategory)
                      .WithMany(e => e.CategoryVariations)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- VariationValue configuration ---
            modelBuilder.Entity<VariationValue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasOne(e => e.CategoryVariation)
                      .WithMany(e => e.VariationValues)
                      .HasForeignKey(e => e.VariationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Product configuration ---
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.Description)
                      .HasColumnType("text");
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10,2)")
                      .HasDefaultValue(0.00m);
                entity.Property(e => e.Qty)
                      .HasDefaultValue(0);
                entity.Property(e => e.ProductImage)
                      .HasMaxLength(255);
                entity.Property(e => e.Sku)
                      .HasMaxLength(50);

                entity.HasOne(e => e.ProductCategory)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of a category that has products
            });

            // --- ProductVariationValue (bridge table) configuration ---
            modelBuilder.Entity<ProductVariationValue>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.ProductVariationValues)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                
                entity.HasOne(e => e.VariationValue)
                      .WithMany(e => e.ProductVariationValues)
                      .HasForeignKey(e => e.VariationValueId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }