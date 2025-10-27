namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Service interface for full CRUD operations on real estate objects.
/// </summary>
public interface IRealEstateObjectCRUDService : IApplicationCrudService<RealEstateObjectDto, RealEstateObjectCreateUpdateDto, int> { }