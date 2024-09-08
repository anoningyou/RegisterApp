
namespace API.DTO.Places;

/// <summary>
/// Represents a province data transfer object.
/// </summary>
public class ProvinceDto : BaseDto
{
    /// <summary>
    /// Gets or sets the name of the province.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    public Guid CountryId { get; set; } 
}
