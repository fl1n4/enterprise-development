namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="RequestDto"/> entities
/// Uses <see cref="RequestCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IRequestCRUDService : IApplicationCrudService<RequestDto, RequestCreateUpdateDto, int> { }