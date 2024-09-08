using API.Dispatchers.Interfaces;
using API.DTO.Places;
using API.Queries.Places;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Represents a controller to handle places.
/// </summary>
/// <param name="dispatcher">Dispatcher.</param>
public class PlacesController(IDispatcher dispatcher) : BaseApiController(dispatcher)
{
    /// <summary>
    /// Gets a list of countries.
    /// </summary>
    /// <param name="query">Filter.</param>
    [HttpGet(nameof(GetCountries))]
    public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries([FromQuery] GetCountriesQuery query)
    {
        return Collection(await QueryAsync(query));
    }

    /// <summary>
    /// Gets a list of provinces.
    /// </summary>
    /// <param name="query">Filter.</param>
    [HttpGet(nameof(GetProvinces))]
    public async Task<ActionResult<IEnumerable<ProvinceDto>>> GetProvinces([FromQuery] GetProvincesQuery query )
    {
        return Collection(await QueryAsync(query));
    }
}
