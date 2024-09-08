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
/// Represents a query handler to get a list of provinces.
/// </summary>
/// <param name="unitOfWork">Unit of work.</param>
/// <param name="mapper">Mapper.</param>
public class GetProvincesQueryHandler (IUnitOfWork<DataContext> unitOfWork, IMapper mapper)
    : IQueryHandler<GetProvincesQuery, List<ProvinceDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepositoryReadonly<Province, Guid> _repository = unitOfWork
        .GetRepositoryReadonly<Province, Guid>();

    ///<inheritdoc/>
    public Task<List<ProvinceDto>> HandleAsync(GetProvincesQuery query)
    {
        Expression<Func<Province, bool>> predicate = ExpressionBuilderExtensions.New<Province>();

        if (query.CountryId != null)
        {
            predicate = predicate.And(p => p.CountryId == query.CountryId);
        }

        if (!string.IsNullOrEmpty(query.NameFilter))
        {
            predicate = predicate.And(p => p.Name.StartsWith(query.NameFilter));
        }

        return _repository.GetListAsync<ProvinceDto>(
            predicate: predicate,
            mapConfig: _mapper.ConfigurationProvider);
    }

}
