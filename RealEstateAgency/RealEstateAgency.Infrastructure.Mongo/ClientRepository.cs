using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Infrastructure.Mongo;

/// <summary>
/// Repository for managing <see cref="Client"/> entities in the MongoDB database
/// Uses the "Clients" collection and inherits basic CRUD functionality from <see cref="MongoRepository{Client, int}"/>
/// </summary>
public class ClientRepository : MongoRepository<Client, int>, IClientRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientRepository"/> class
    /// </summary>
    /// <param name="database">The MongoDB database instance</param>
    public ClientRepository(IMongoDatabase database)
        : base(database, "Clients")
    {
    }
}