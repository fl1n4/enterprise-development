using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Domain;

/// <summary>
/// Interface for managing <see cref="Client"/> entities in the repository
/// Inherits CRUD operations from <see cref="IRepository{Client, int}"/>
/// </summary>
public interface IClientRepository : IRepository<Client, int> { }