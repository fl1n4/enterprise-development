using RealEstateAgency.Application.Contracts;

namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Service interface for full CRUD operations on clients.
/// </summary>
public interface IClientCRUDService : IApplicationReadService<ClientDto, int>
{
    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="dto">The client data to create.</param>
    /// <returns>The created client DTO.</returns>
    public Task<ClientDto> Create(ClientCreateUpdateDto dto);

    /// <summary>
    /// Updates an existing client.
    /// </summary>
    /// <param name="id">The ID of the client to update.</param>
    /// <param name="dto">The updated client data.</param>
    /// <returns>The updated client DTO.</returns>
    public Task<ClientDto> Update(int id, ClientCreateUpdateDto dto);

    /// <summary>
    /// Deletes a client by ID.
    /// </summary>
    /// <param name="id">The ID of the client to delete.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    public Task Delete(int id);
}