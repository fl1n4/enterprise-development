using MapsterMapper;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing full CRUD operations for <see cref="RealEstateObject"/> entities.
/// Implements <see cref="IRealEstateObjectCRUDService"/>.
/// </summary>
public class RealEstateObjectService(
    IRepository<RealEstateObject, int> repository,
    IMapper mapper)
    : IRealEstateObjectCRUDService
{
    private static int _nextId = 1;

    public async Task<RealEstateObjectDto> Create(RealEstateObjectCreateUpdateDto dto)
    {
        var entity = mapper.Map<RealEstateObject>(dto);
        entity.Id = Interlocked.Increment(ref _nextId); // временная генерация ID
        var created = await repository.Create(entity);
        return mapper.Map<RealEstateObjectDto>(created);
    }

    public async Task<RealEstateObjectDto> Update(RealEstateObjectCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Real estate object with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<RealEstateObjectDto>(updated);
    }

    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    public async Task<RealEstateObjectDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Real estate object with ID {dtoId} not found");
        return mapper.Map<RealEstateObjectDto>(entity);
    }

    public async Task<IList<RealEstateObjectDto>> GetAll() =>
        mapper.Map<List<RealEstateObjectDto>>(await repository.GetAll());
}