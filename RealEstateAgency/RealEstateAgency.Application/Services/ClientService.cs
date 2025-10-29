using MapsterMapper;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service for managing <see cref="Client"/> entities
/// Implements CRUD operations using <see cref="IClientRepository"/> and object mapping through <see cref="IMapper"/>
/// </summary>
public class ClientService(
    IClientRepository repository,
    IMapper mapper)
    : IClientCRUDService
{

    /// <summary>
    /// Creates a new client entity
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="ClientDto"/> entity</returns>
    public async Task<ClientDto> Create(ClientCreateUpdateDto dto)
    {
        var entity = mapper.Map<Client>(dto);

        // Генерация нового Id
        var allClients = await repository.GetAll();
        var lastId = allClients.Any() ? allClients.Max(c => c.Id) : 0;
        entity.Id = lastId + 1;

        var created = await repository.Create(entity);
        return mapper.Map<ClientDto>(created);
    }

    /// <summary>
    /// Updates an existing client entity
    /// </summary>
    /// <param name="dto">The data transfer object with updated data</param>
    /// <param name="dtoId">The identifier of the client to update</param>
    /// <returns>The updated <see cref="ClientDto"/> entity</returns>
    public async Task<ClientDto> Update(ClientCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<ClientDto>(updated);
    }

    /// <summary>
    /// Deletes a client by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the client to delete</param>
    /// <returns>True if deletion was successful, otherwise false</returns>
    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Retrieves a client by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the client</param>
    /// <returns>The corresponding <see cref="ClientDto"/> entity</returns>
    public async Task<ClientDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        return mapper.Map<ClientDto>(entity);
    }

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>A list of <see cref="ClientDto"/> entities</returns>
    public async Task<IList<ClientDto>> GetAll() =>
        mapper.Map<List<ClientDto>>(await repository.GetAll());
}