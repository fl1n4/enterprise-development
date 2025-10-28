using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Domain;

/// <summary>
/// Interface for managing <see cref="RealEstateObject"/> entities in the repository
/// Inherits CRUD operations from <see cref="IRepository{RealEstateObject, int}"/>
/// </summary>
public interface IRealEstateObjectRepository : IRepository<RealEstateObject, int> { }