using MapsterMapper;
using RealEstateAgency.Application.Contracts;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing read operations for <see cref="Request"/> entities.
/// Implements <see cref="IApplicationReadService{RequestDto, int}"/>.
/// </summary>
public class RequestService(
    IRepository<Request, int> repository,
    IMapper mapper)
    : IApplicationReadService<RequestDto, int>
{
    public async Task<RequestDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Request with ID {dtoId} not found");
        return mapper.Map<RequestDto>(entity);
    }

    public async Task<IList<RequestDto>> GetAll() =>
        mapper.Map<List<RequestDto>>(await repository.GetAll());
}