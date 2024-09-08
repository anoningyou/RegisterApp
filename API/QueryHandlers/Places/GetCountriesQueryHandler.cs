using System.Linq.Expressions;
using API.Data;
using API.Data.Interfaces;
using API.DTO.Places;
using API.Entities.Places;
using API.Extensions;
using API.Queries.Places;
using API.QueryHandlers.Interfaces;
using AutoMapper;

namespace API.QueryHandlers.Places;

/// <summary>
/// Represents a query handler to get a list of countries.
/// </summary>
/// <param name="unitOfWork">Unit of work.</param>
/// <param name="mapper">Mapper.</param>
public class GetCountriesQueryHandler (IUnitOfWork<DataContext> unitOfWork, IMapper mapper)
    : IQueryHandler<GetCountriesQuery, List<CountryDto>>
{

    private readonly IMapper _mapper = mapper;
    private readonly IRepositoryReadonly<Country, Guid> _repository = unitOfWork
        .GetRepositoryReadonly<Country, Guid>();

    ///<inheritdoc/>
    public Task<List<CountryDto>> HandleAsync(GetCountriesQuery query)
    {
        Expression<Func<Country, bool>> predicate = ExpressionBuilderExtensions.New<Country>();

        if (!string.IsNullOrEmpty(query.NameFilter))
        {
            predicate = predicate.And(p => p.Name.StartsWith(query.NameFilter));
        }

        return _repository.GetListAsync<CountryDto>(
            predicate: predicate,
            mapConfig: _mapper.ConfigurationProvider);
    }

}
