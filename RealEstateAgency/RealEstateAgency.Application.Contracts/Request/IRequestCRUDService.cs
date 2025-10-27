namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Service interface for full CRUD operations on requests.
/// </summary>
public interface IRequestCRUDService : IApplicationCrudService<RequestDto, RequestCreateUpdateDto, int> { }