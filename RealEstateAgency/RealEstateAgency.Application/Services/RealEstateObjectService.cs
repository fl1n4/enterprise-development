using MapsterMapper;
using RealEstateAgency.Application.Contracts;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing read operations for <see cref="RealEstateObject"/> entities.
/// Implements <see cref="IApplicationReadService{RealEstateObjectDto, int}"/>.
/// </summary>
public class RealEstateObjectService(
    IRepository<RealEstateObject, int> repository,
    IMapper mapper)
    : IApplicationReadService<RealEstateObjectDto, int>
{
    public async Task<RealEstateObjectDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Real estate object with ID {dtoId} not found");
        return mapper.Map<RealEstateObjectDto>(entity);
    }

    public async Task<IList<RealEstateObjectDto>> GetAll() =>
        mapper.Map<List<RealEstateObjectDto>>(await repository.GetAll());
}