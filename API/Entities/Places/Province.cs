using System.Collections.ObjectModel;
using API.Entities.Accounting;

namespace API.Entities.Places;

/// <summary>
/// Represents a province.
/// </summary>
public class Province : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the province.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    public Guid CountryId { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public virtual Country Country { get; set; }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    public virtual Collection<AppUser> Users { get; set; }

}
