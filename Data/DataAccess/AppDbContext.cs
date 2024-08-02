using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data.DataAccess;

public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions options) : base(options) 
    {

    }



    [Obsolete]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(b =>
        {
            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<Role>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        modelBuilder.Entity<Role>().HasData(
            new Entities.Role()
            {
                Id = Guid.Parse("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Role for Admin"
            });
        modelBuilder.Entity<Role>().HasData(
            new Entities.Role()
            {
                Id = Guid.Parse("7119a2e7-e680-4ecd-8344-0c53082cdc87"),
                Name = "Manager",
                NormalizedName = "MANAGER",
                Description = "Role for Manager"
            });
        modelBuilder.Entity<Role>().HasData(
            new Entities.Role()
            {
                Id = Guid.Parse("931c6340-f21a-4bbf-b71c-e39d7cebc997"),
                Name = "Staff",
                NormalizedName = "STAFF",
                Description = "Role for Staff"
            });
        modelBuilder.Entity<User>().HasData(
            new Entities.User
            {
                Id = Guid.Parse("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                FirstName = "System",
                LastName = "Admin",
                IsDeleted = false,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "lieudvq0302@gmail.com",
                NormalizedEmail = "LIEUDVQ0302@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEGAV0YwJmXtNmREj0oD2OX9feN5dC0WIPykTsTLuLhOCpJUPZSBQQww0K/IJ4v7NRw==",
                EmailConfirmed = false,
                IsActive = true,
                SecurityStamp = "SMERHXAHGAGZWB7SKS2FZAH3PHQC7WXL",
                ConcurrencyStamp = "8a12fe29-7fe8-41f3-9af2-54b1ca8d4207",
                PhoneNumber = "012345609",
                LockoutEnabled = true,
                AccessFailedCount = 0,

            });
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole{
                UserId = Guid.Parse("c48fa0b7-47e0-4af2-bb56-3db9e29a7e8b"),
                RoleId = Guid.Parse("003f7676-1d91-4143-9bfd-7a6c17c156fe"),
            });


        modelBuilder.Entity<UomUom>(entity =>
        {
            entity.HasCheckConstraint("uom_uom_factor_gt_zero", "\"Factor\" <> 0");
            entity.HasCheckConstraint("uom_uom_factor_reference_is_one", "((\"UomType\" = 'reference' AND \"Factor\" = 1.0) OR (\"UomType\" <> 'reference'))");
            entity.HasCheckConstraint("uom_uom_rounding_gt_zero", "\"Rounding\" > 0");
        });
        modelBuilder.Entity<UomUom>()
            .HasOne(r => r.Category)
            .WithMany(u => u.UomUoms)
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(r => r.ProductRemoval)
            .WithMany(u => u.ProductCategories)
            .HasForeignKey(r => r.RemovalStrategyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductAttributeValue>()
            .HasOne(r => r.ProductAttribute)
            .WithMany(u => u.ProductAttributeValues)
            .HasForeignKey(r => r.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductTemplate>()
            .HasOne(r => r.UomUom)
            .WithMany(u => u.ProductTemplates)
            .HasForeignKey(r => r.UomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductProduct>()
            .HasOne(r => r.ProductTemplate)
            .WithMany(u => u.ProductProducts)
            .HasForeignKey(r => r.ProductTmplId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductTemplateAttributeLine>()
            .HasOne(ptal => ptal.ProductTemplate)
            .WithMany(pt => pt.ProductTemplateAttributeLines)
            .HasForeignKey(ptal => ptal.ProductTmplId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductTemplateAttributeLine>()
            .HasOne(ptal => ptal.ProductAttribute)
            .WithMany(pa => pa.ProductTemplateAttributeLines)
            .HasForeignKey(ptal => ptal.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductTemplateAttributeValue>()
            .HasOne(ptal => ptal.ProductAttributeValue)
            .WithMany(pt => pt.ProductTemplateAttributeValues)
            .HasForeignKey(ptal => ptal.ProductAttributeValueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductTemplateAttributeValue>()
            .HasOne(ptal => ptal.ProductTemplateAttributeLine)
            .WithMany(pa => pa.ProductTemplateAttributeValues)
            .HasForeignKey(ptal => ptal.AttributeLineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductVariantCombination>()
            .HasOne(ptal => ptal.ProductProduct)
            .WithMany(pa => pa.ProductVariantCombinations)
            .HasForeignKey(ptal => ptal.ProductProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductVariantCombination>()
            .HasOne(ptal => ptal.ProductTemplateAttributeValue)
            .WithMany(pt => pt.ProductVariantCombinations)
            .HasForeignKey(ptal => ptal.ProductTemplateAttributeValueId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockQuant>()
            .HasOne(r => r.ProductProduct)
            .WithMany(u => u.StockQuants)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockMoveLine>()
            .HasOne(r => r.StockMove)
            .WithMany(u => u.StockMoveLines)
            .HasForeignKey(r => r.MoveId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockMoveLine>()
            .HasOne(r => r.StockQuant)
            .WithMany(u => u.StockMoveLines)
            .HasForeignKey(r => r.QuantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockQuant>()
            .HasOne(r => r.ProductProduct)
            .WithMany(u => u.StockQuants)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockQuant>()
            .HasOne(r => r.StockLocation)
            .WithMany(u => u.StockQuants)
            .HasForeignKey(r => r.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockLocation>()
            .HasMany(r => r.StockQuants)
            .WithOne(u => u.StockLocation)
            .HasForeignKey(r => r.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        var virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
        var inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
        var physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
        var partnerLocationId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479");
        var vendorLocationId = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        modelBuilder.Entity<StockLocation>().HasData(
            new Entities.StockLocation()
            {
                Id = virtualLocationId,
                Name = "Virtual Locations",
                CompleteName = "Virtual Locations",
                ParentPath = $"{virtualLocationId}/",
                Usage = Enums.LocationType.View,
            });

        modelBuilder.Entity<StockLocation>().HasData(
            new Entities.StockLocation()
            {
                Id = inventoryAdjustmentId,
                LocationId = virtualLocationId,
                Name = "Inventory adjustment",
                CompleteName = "Virtual Locations / Inventory adjustment",
                ParentPath = $"{virtualLocationId}/{inventoryAdjustmentId}/",
                Usage = Enums.LocationType.View,
            });
        modelBuilder.Entity<StockLocation>().HasData(
            new Entities.StockLocation()
            {
                Id = physicalLocationId,
                Name = "Physical Locations",
                CompleteName = "Physical Locations",
                ParentPath = $"{physicalLocationId}/",
                Usage = Enums.LocationType.View,
            });
        modelBuilder.Entity<StockLocation>().HasData(
            new Entities.StockLocation()
            {
                Id = partnerLocationId,
                Name = "Partners",
                CompleteName = "Partners",
                ParentPath = $"{partnerLocationId}/",
                Usage = Enums.LocationType.View,
            });
        modelBuilder.Entity<StockLocation>().HasData(
            new Entities.StockLocation()
            {
                Id = vendorLocationId,
                LocationId = partnerLocationId,
                Name = "Vendors",
                CompleteName = "Partners / Vendors",
                ParentPath = $"{partnerLocationId}/{vendorLocationId}/",
                Usage = Enums.LocationType.Supplier,
            });
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<Role> Role { get; set; }

    public DbSet<UomCategory> UomCategory { get; set; }
    public DbSet<UomUom> UomUom { get; set; }

    public DbSet<ProductRemoval> ProductRemoval { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }

    public DbSet<ProductAttribute> ProductAttribute { get; set; }
    public DbSet<ProductAttributeValue> ProductAttributeValue { get; set; }

    public DbSet<ProductTemplate> ProductTemplate { get; set; }
    public DbSet<ProductProduct> ProductProduct { get; set; }

    public DbSet<ProductTemplateAttributeLine> ProductTemplateAttributeLine { get; set; }
    public DbSet<ProductTemplateAttributeValue> ProductTemplateAttributeValue { get; set; }
    public DbSet<ProductVariantCombination> ProductVariantCombination { get; set; }

    public DbSet<StockWarehouse> StockWarehouse { get; set; }
    public DbSet<StockLocation> StockLocation { get; set; }
    public DbSet<StockPickingType> StockPickingType { get; set; }

    public DbSet<StockQuant> StockQuant { get; set; }
    public DbSet<StockMoveLine> StockMoveLine { get; set; }
    public DbSet<StockMove> StockMove { get; set; }
    public DbSet<StockPicking> StockPicking { get; set; }

}
