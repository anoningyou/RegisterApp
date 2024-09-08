namespace API.Entities.Places;

/// <summary>
/// Represents a country.
/// </summary>
public class Country: BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the country.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the provinces.
    /// </summary>
    public virtual ICollection<Province> Provinces { get; set; }
}
