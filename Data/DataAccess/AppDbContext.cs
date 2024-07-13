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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  
    }
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<Role> Role { get; set; }

    public DbSet<UomCategory> UomCategory { get; set; }
    public DbSet<UomUom> UomUom { get; set; }

}
