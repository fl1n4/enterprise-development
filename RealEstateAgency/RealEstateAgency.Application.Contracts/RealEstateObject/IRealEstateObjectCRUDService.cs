namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="RealEstateObjectDto"/> entities
/// Uses <see cref="RealEstateObjectCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IRealEstateObjectCRUDService : IApplicationCrudService<RealEstateObjectDto, RealEstateObjectCreateUpdateDto, int> { }