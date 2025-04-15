using Microsoft.EntityFrameworkCore;
using ShippingAPI.ApplicationCore.Entities;

namespace ShippingAPI.Infrastructure.Data;

public class ShippingDbContext : DbContext
    {
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Shipper> Shippers { get; set; } = null!;
        public DbSet<ShipperRegion> ShipperRegions { get; set; } = null!;
        public DbSet<ShippingDetails> ShippingDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the primary keys
            modelBuilder.Entity<Region>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Shipper>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<ShippingDetails>()
                .HasKey(sd => sd.Id);

            // ShipperRegion: Composite Key
            modelBuilder.Entity<ShipperRegion>()
                .HasKey(sr => new { sr.ShipperId, sr.RegionId });

            // Relationship: ShipperRegion -> Shipper
            modelBuilder.Entity<ShipperRegion>()
                .HasOne(sr => sr.Shipper)
                .WithMany(s => s.ShipperRegions)
                .HasForeignKey(sr => sr.ShipperId);

            // Relationship: ShipperRegion -> Region
            modelBuilder.Entity<ShipperRegion>()
                .HasOne(sr => sr.Region)
                .WithMany(r => r.ShipperRegions)
                .HasForeignKey(sr => sr.RegionId);

            // Relationship: ShippingDetails -> Shipper
            modelBuilder.Entity<ShippingDetails>()
                .HasOne(sd => sd.Shipper)
                .WithMany(s => s.ShippingDetails)
                .HasForeignKey(sd => sd.ShipperId);
        }
    }