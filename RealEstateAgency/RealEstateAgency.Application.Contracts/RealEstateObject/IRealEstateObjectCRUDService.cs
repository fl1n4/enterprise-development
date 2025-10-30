using RealEstateAgency.Application.Contracts.Request;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="RealEstateObjectDto"/> entities
/// Uses <see cref="RealEstateObjectCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IRealEstateObjectCRUDService : IApplicationCrudService<RealEstateObjectDto, RealEstateObjectCreateUpdateDto, int> 
{
    /// <summary>
    /// Retrieves all requests associated with a specific property.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the property whose requests are to be retrieved.</param>
    /// <returns>A list of <see cref="RequestDto"/> representing the property's requests.</returns>
    public Task<IList<RequestDto>> GetRequestsByPropertyId(int propertyId);
}