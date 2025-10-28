using MapsterMapper;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service for managing <see cref="Request"/> entities
/// Implements CRUD operations using <see cref="IRequestRepository"/> and object mapping through <see cref="IMapper"/>
/// </summary>
public class RequestService(
    IRequestRepository repository,
    IMapper mapper)
    : IRequestCRUDService
{
    private static int _nextId = 1;

    /// <summary>
    /// Creates a new request entity
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="RequestDto"/> entity</returns>
    public async Task<RequestDto> Create(RequestCreateUpdateDto dto)
    {
        var entity = mapper.Map<Request>(dto);
        entity.Id = Interlocked.Increment(ref _nextId);
        var created = await repository.Create(entity);
        return mapper.Map<RequestDto>(created);
    }

    /// <summary>
    /// Updates an existing request entity
    /// </summary>
    /// <param name="dto">The data transfer object with updated data</param>
    /// <param name="dtoId">The identifier of the request to update</param>
    /// <returns>The updated <see cref="RequestDto"/> entity</returns>
    public async Task<RequestDto> Update(RequestCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Request with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<RequestDto>(updated);
    }

    /// <summary>
    /// Deletes a request by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the request to delete</param>
    /// <returns>True if deletion was successful, otherwise false</returns>
    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Retrieves a request by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the request</param>
    /// <returns>The corresponding <see cref="RequestDto"/> entity</returns>
    public async Task<RequestDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Request with ID {dtoId} not found");
        return mapper.Map<RequestDto>(entity);
    }

    /// <summary>
    /// Retrieves all requests
    /// </summary>
    /// <returns>A list of <see cref="RequestDto"/> entities</returns>
    public async Task<IList<RequestDto>> GetAll() =>
        mapper.Map<List<RequestDto>>(await repository.GetAll());
}