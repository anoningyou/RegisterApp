using API.DTO.Accounting;
using API.DTO.Places;
using API.Entities.Accounting;
using API.Entities.Places;
using AutoMapper;

namespace API.Settings;

/// <summary>
/// AutoMapper profile class for mapping between different types.
/// </summary>
public class AutoMapperProfiles : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperProfiles"/> class.
    /// Configures the mappings between different types.
    /// </summary>
    public AutoMapperProfiles()
    {
        CreateMap<DateTime, DateTime>()
            .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));

        CreateMap<DateTime?, DateTime?>()
            .ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

        CreateMap<RegisterDto, AppUser>();

        CreateMap<Province, ProvinceDto>().ReverseMap();

        CreateMap<Country, CountryDto>().ReverseMap();
        
    }
}