using API.Data.Interfaces;
using API.Entities.Places;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Accounting;

/// <summary>
/// Represents an application user.
/// </summary>
public class AppUser : IdentityUser<Guid>, IIdentifiable<Guid>
{
    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the user was last active.
    /// </summary>
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collection of user roles associated with the user.
    /// </summary>
    public virtual ICollection<AppUserRole> UserRoles { get; set; }

    /// <summary>
    /// Gets or sets the Province.
    /// </summary>
    public virtual Province Province { get; set; }
}