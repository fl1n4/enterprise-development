using MapsterMapper;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service for managing <see cref="RealEstateObject"/> entities
/// Implements CRUD operations using <see cref="IRealEstateObjectRepository"/> and object mapping through <see cref="IMapper"/>
/// </summary>
public class RealEstateObjectService(
    IRealEstateObjectRepository repository,
    IMapper mapper)
    : IRealEstateObjectCRUDService
{
    private static int _nextId = 1;

    /// <summary>
    /// Creates a new real estate object entity
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="RealEstateObjectDto"/> entity</returns>
    public async Task<RealEstateObjectDto> Create(RealEstateObjectCreateUpdateDto dto)
    {
        var entity = mapper.Map<RealEstateObject>(dto);
        entity.Id = Interlocked.Increment(ref _nextId);
        var created = await repository.Create(entity);
        return mapper.Map<RealEstateObjectDto>(created);
    }

    /// <summary>
    /// Updates an existing real estate object entity
    /// </summary>
    /// <param name="dto">The data transfer object with updated data</param>
    /// <param name="dtoId">The identifier of the real estate object to update</param>
    /// <returns>The updated <see cref="RealEstateObjectDto"/> entity</returns>
    public async Task<RealEstateObjectDto> Update(RealEstateObjectCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Real estate object with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<RealEstateObjectDto>(updated);
    }

    /// <summary>
    /// Deletes a real estate object by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the real estate object to delete</param>
    /// <returns>True if deletion was successful, otherwise false</returns>
    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Retrieves a real estate object by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the real estate object</param>
    /// <returns>The corresponding <see cref="RealEstateObjectDto"/> entity</returns>
    public async Task<RealEstateObjectDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Real estate object with ID {dtoId} not found");
        return mapper.Map<RealEstateObjectDto>(entity);
    }

    /// <summary>
    /// Retrieves all real estate objects
    /// </summary>
    /// <returns>A list of <see cref="RealEstateObjectDto"/> entities</returns>
    public async Task<IList<RealEstateObjectDto>> GetAll() =>
        mapper.Map<List<RealEstateObjectDto>>(await repository.GetAll());
}