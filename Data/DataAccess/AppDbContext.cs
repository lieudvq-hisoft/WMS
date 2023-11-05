using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.DataAccess;

public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions options) : base(options) 
    {

    }
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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  
    }
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Supplier> Supplier { get; set; }
    public DbSet<Receipt> Receipt { get; set; }
    public DbSet<ReceiptInventory> ReceiptInventory { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Rack> Rack { get; set; }
    public DbSet<RackLevel> RackLevel { get; set; }
    public DbSet<PickingRequest> PickingRequest { get; set; }
    public DbSet<PickingRequestInventory> PickingRequestInventory { get; set; }
}
