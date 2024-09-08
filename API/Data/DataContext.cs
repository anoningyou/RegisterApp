using API.Entities.Accounting;
using API.Entities.Places;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, Guid,
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{
    /// <summary>
    /// Gets or sets the Countries in the database.
    /// </summary>
    public DbSet<Country> Countries { get; set; }

    /// <summary>
    /// Gets or sets the Provinces in the database.
    /// </summary>
    public DbSet<Province> Provinces { get; set; }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
        
        builder.Entity<AppUser>()
            .HasOne(u => u.Province)
            .WithMany(r => r.Users);
        
        builder.Entity<AppRole>()
            .HasMany(u => u.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

        builder.Entity<Country>()
            .HasMany(p => p.Provinces)
            .WithOne(c => c.Country)
            .HasForeignKey(p => p.CountryId)
            .IsRequired();

        builder.Entity<Country>()
            .HasIndex(c => c.Name, "IX_Country_Name")
            .IsUnique();

        builder.Entity<Province>()
            .HasIndex(p => new {p.CountryId, p.Name}, "IX_Province_CountryId_Name")
            .IsUnique();
    
    }
}