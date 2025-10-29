using MapsterMapper;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;

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

    /// <summary>
    /// Creates a new request entity
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="RequestDto"/> entity</returns>
    public async Task<RequestDto> Create(RequestCreateUpdateDto dto)
    {
        var entity = mapper.Map<Request>(dto);

        var allRequests = await repository.GetAll();
        var lastId = allRequests.Any() ? allRequests.Max(r => r.Id) : 0;
        entity.Id = lastId + 1;

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

    /// <summary>
    /// Retrieves the client associated with a specific request
    /// </summary>
    public async Task<ClientDto> GetClientByRequestId(int requestId)
    {
        var request = await repository.Get(requestId)
                      ?? throw new KeyNotFoundException($"Request with ID {requestId} not found");

        var client = request.Client;
        return mapper.Map<ClientDto>(client);
    }

    /// <summary>
    /// Retrieves the property associated with a specific request
    /// </summary>
    public async Task<RealEstateObjectDto> GetPropertyByRequestId(int requestId)
    {
        var request = await repository.Get(requestId)
                      ?? throw new KeyNotFoundException($"Request with ID {requestId} not found");

        var property = request.Property;
        return mapper.Map<RealEstateObjectDto>(property);
    }

    /// <summary>
    /// Counts requests grouped by property type
    /// </summary>
    public async Task<Dictionary<PropertyType, int>> GetRequestCountByPropertyType()
    {
        var requests = await repository.GetAll();
        return requests
            .GroupBy(r => r.Property.Type)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}