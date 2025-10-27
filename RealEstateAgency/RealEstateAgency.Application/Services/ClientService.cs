using MapsterMapper;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing full CRUD operations for <see cref="Client"/> entities.
/// Implements <see cref="IClientCRUDService"/>.
/// </summary>
public class ClientService(
    IRepository<Client, int> repository,
    IMapper mapper)
    : IClientCRUDService
{
    public async Task<ClientDto> Create(ClientCreateUpdateDto dto)
    {
        var entity = mapper.Map<Client>(dto);
        entity.Id = GenerateNewId();
        var created = await repository.Create(entity);
        return mapper.Map<ClientDto>(created);
    }

    public async Task<ClientDto> Update(ClientCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<ClientDto>(updated);
    }

    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    public async Task<ClientDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        return mapper.Map<ClientDto>(entity);
    }

    public async Task<IList<ClientDto>> GetAll() =>
        mapper.Map<List<ClientDto>>(await repository.GetAll());

    private static int _nextId = 1;
    private int GenerateNewId() => Interlocked.Increment(ref _nextId);
}