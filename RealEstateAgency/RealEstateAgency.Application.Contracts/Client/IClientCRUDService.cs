namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="ClientDto"/> entities
/// Uses <see cref="ClientCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IClientCRUDService : IApplicationCrudService<ClientDto, ClientCreateUpdateDto, int>{}