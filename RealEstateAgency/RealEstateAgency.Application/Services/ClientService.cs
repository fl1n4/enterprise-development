using RealEstateAgency.Application.Contracts;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Domain.Entities;
using MapsterMapper;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing read operations for <see cref="Client"/> entities.
/// Implements <see cref="IApplicationReadService{ClientDto, int}"/>.
/// </summary>
public class ClientService(
    IRepository<Client, int> repository,
    IMapper mapper)
    : IApplicationReadService<ClientDto, int>
{
    /// <summary>
    /// Retrieves a single <see cref="ClientDto"/> by its unique identifier.
    /// </summary>
    /// <param name="dtoId">The ID of the client to retrieve.</param>
    /// <returns>The <see cref="ClientDto"/> corresponding to the given ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if no client with the specified ID exists.</exception>
    public async Task<ClientDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Client with ID {dtoId} not found");
        return mapper.Map<ClientDto>(entity);
    }

    /// <summary>
    /// Retrieves all <see cref="ClientDto"/> entities from the repository.
    /// </summary>
    /// <returns>A list of all client DTOs.</returns>
    public async Task<IList<ClientDto>> GetAll() =>
        mapper.Map<List<ClientDto>>(await repository.GetAll());
}