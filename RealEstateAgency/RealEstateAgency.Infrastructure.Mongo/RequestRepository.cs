using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Infrastructure.Mongo;

/// <summary>
/// Repository for managing <see cref="Request"/> entities in the MongoDB database
/// Uses the "Requests" collection and inherits basic CRUD functionality from <see cref="MongoRepository{Request, int}"/>
/// </summary>
public class RequestRepository : MongoRepository<Request, int>, IRequestRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestRepository"/> class
    /// </summary>
    /// <param name="database">The MongoDB database instance</param>
    public RequestRepository(IMongoDatabase database)
        : base(database, "Requests")
    {
    }
}