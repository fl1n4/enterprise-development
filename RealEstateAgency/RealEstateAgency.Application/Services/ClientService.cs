using MapsterMapper;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service for managing <see cref="Client"/> entities
/// Implements CRUD operations using <see cref="IClientRepository"/> and object mapping through <see cref="IMapper"/>
/// </summary>
public class ClientService(
    IClientRepository ClientRepository,
    IRequestRepository RequestRepository,
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

        var allClients = await ClientRepository.GetAll();
        var lastId = allClients.Any() ? allClients.Max(c => c.Id) : 0;
        entity.Id = lastId + 1;

        var created = await ClientRepository.Create(entity);
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
        var existing = await ClientRepository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await ClientRepository.Update(updatedEntity);
        return mapper.Map<ClientDto>(updated);
    }

    /// <summary>
    /// Deletes a client by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the client to delete</param>
    /// <returns>True if deletion was successful, otherwise false</returns>
    public async Task<bool> Delete(int dtoId)
    {
        return await ClientRepository.Delete(dtoId);
    }

    /// <summary>
    /// Retrieves a client by its identifier
    /// </summary>
    /// <param name="dtoId">The identifier of the client</param>
    /// <returns>The corresponding <see cref="ClientDto"/> entity</returns>
    public async Task<ClientDto> Get(int dtoId)
    {
        var entity = await ClientRepository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        return mapper.Map<ClientDto>(entity);
    }

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>A list of <see cref="ClientDto"/> entities</returns>
    public async Task<IList<ClientDto>> GetAll() =>
        mapper.Map<List<ClientDto>>(await ClientRepository.GetAll());

    /// <summary>
    /// Retrieves clients who made sell requests within the specified period
    /// </summary>
    public async Task<IList<ClientDto>> GetSellersByPeriod(DateOnly from, DateOnly to)
    {
        var requests = await RequestRepository.GetRequests();
        var sellers = requests
            .Where(r => r.Type == RequestType.Sell
                        && r.DateCreated >= from
                        && r.DateCreated <= to)
            .Select(r => r.Client)
            .Distinct()
            .ToList();

        return mapper.Map<List<ClientDto>>(sellers);
    }

    /// <summary>
    /// Retrieves top 5 clients grouped by request type (Buy/Sell)
    /// </summary>
    public async Task<Dictionary<RequestType, List<ClientDto>>> GetTop5ClientsByRequestType()
    {
        var requests = await RequestRepository.GetRequests();

        var groupedTopClients = requests
    .Where(r => r.Type != null)
    .GroupBy(r => r.Type!.Value)
    .ToDictionary(
        g => g.Key,
        g => g.GroupBy(r => r.Client)
              .Select(cg => new { Client = cg.Key, Count = cg.Count() })
              .OrderByDescending(x => x.Count)
              .Take(5)
              .Select(x => x.Client)
              .ToList()
    );

        return groupedTopClients.ToDictionary(
            kvp => kvp.Key,
            kvp => mapper.Map<List<ClientDto>>(kvp.Value)
        );
    }

    /// <summary>
    /// Retrieves clients who submitted requests with the minimum amount
    /// </summary>
    public async Task<IList<ClientDto>> GetClientsWithMinRequestAmount()
    {
        var requests = await RequestRepository.GetRequests();
        var minAmount = requests.Min(r => r.Amount);

        var clients = requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client)
            .Distinct()
            .ToList();

        return mapper.Map<List<ClientDto>>(clients);
    }

    /// <summary>
    /// Retrieves clients who submitted buy requests for a specific property type
    /// </summary>
    public async Task<IList<ClientDto>> GetClientsByPropertyType(PropertyType type)
    {
        var requests = await RequestRepository.GetRequests();

        var clients = requests
            .Where(r => r.Property != null && r.Type == RequestType.Buy && r.Property.Type == type)
            .Select(r => r.Client)
            .Where(c => c != null)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        return mapper.Map<List<ClientDto>>(clients);
    }

    /// <summary>
    /// Retrieves all requests made by a specific client
    /// </summary>
    /// <param name="clientId">The identifier of the client</param>
    /// <returns>List of <see cref="RequestDto"/> associated with the client</returns>
    public async Task<IList<RequestDto>> GetRequestsByClientId(int clientId)
    {
        var requests = await RequestRepository.GetRequests();
        var clientRequests = requests
            .Where(r => r.Client.Id == clientId)
            .ToList();

        return mapper.Map<List<RequestDto>>(clientRequests);
    }
}