namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Service interface for full CRUD operations on clients.
/// </summary>
public interface IClientCRUDService : IApplicationCrudService<ClientDto, ClientCreateUpdateDto, int>{}