using API.DTO.Places;
using API.Queries.Interfaces;

namespace API.Queries.Places;

/// <summary>
/// Represents a query to get a list of countries.
/// </summary>
public class GetCountriesQuery : IQuery<List<CountryDto>>
{
    /// <summary>
    /// Gets or sets the name filter.
    /// </summary>
    public string NameFilter { get; set; }
}
