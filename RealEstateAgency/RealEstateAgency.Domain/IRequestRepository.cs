using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Domain;

/// <summary>
/// Interface for managing <see cref="Request"/> entities in the repository
/// Inherits CRUD operations from <see cref="IRepository{Request, int}"/>
/// </summary>
public interface IRequestRepository : IRepository<Request, int> 
{
    /// <summary>
    /// Retrieves all request entities from the repository.
    /// </summary>
    /// <returns>A list of all <see cref="Request"/> entities.</returns>
    public Task<IList<Request>> GetRequests() => GetAll();
}