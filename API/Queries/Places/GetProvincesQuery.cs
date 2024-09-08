using API.DTO.Places;
using API.Queries.Interfaces;

namespace API.Queries.Places;

/// <summary>
/// Represents a query to get a list of provinces.
/// </summary>
public class GetProvincesQuery : IQuery<List<ProvinceDto>>
{
    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    public Guid? CountryId { get; set; } = null;

    /// <summary>
    /// Gets or sets the name filter.
    /// </summary>
    public string NameFilter { get; set; } = null;

}
