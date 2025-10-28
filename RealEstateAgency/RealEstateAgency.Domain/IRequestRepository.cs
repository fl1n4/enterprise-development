using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Domain;

/// <summary>
/// Interface for managing <see cref="Request"/> entities in the repository
/// Inherits CRUD operations from <see cref="IRepository{Request, int}"/>
/// </summary>
public interface IRequestRepository : IRepository<Request, int> { }