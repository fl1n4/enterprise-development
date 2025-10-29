using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="RequestDto"/> entities
/// Uses <see cref="RequestCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IRequestCRUDService : IApplicationCrudService<RequestDto, RequestCreateUpdateDto, int> 
{
    /// <summary>
    /// Retrieves the client associated with a specific request.
    /// </summary>
    /// <param name="requestId">The unique identifier of the request.</param>
    /// <returns>The corresponding <see cref="ClientDto"/> entity.</returns>
    public Task<ClientDto> GetClientByRequestId(int requestId);

    /// <summary>
    /// Retrieves the real estate property associated with a specific request.
    /// </summary>
    /// <param name="requestId">The unique identifier of the request.</param>
    /// <returns>The corresponding <see cref="RealEstateObjectDto"/> entity.</returns>
    public Task<RealEstateObjectDto> GetPropertyByRequestId(int requestId);

    /// <summary>
    /// Retrieves a count of requests grouped by property type.
    /// </summary>
    /// <returns>
    /// A dictionary where the key is a <see cref="PropertyType"/>, 
    /// and the value is the number of requests for that type.
    /// </returns>
    public Task<Dictionary<PropertyType, int>> GetRequestCountByPropertyType();
}