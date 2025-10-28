using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Infrastructure.Mongo;

/// <summary>
/// Repository for managing <see cref="RealEstateObject"/> entities in the MongoDB database
/// Uses the "RealEstateObjects" collection and inherits basic CRUD functionality from <see cref="MongoRepository{RealEstateObject, int}"/>
/// </summary>
public class RealEstateObjectRepository : MongoRepository<RealEstateObject, int>, IRealEstateObjectRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RealEstateObjectRepository"/> class
    /// </summary>
    /// <param name="database">The MongoDB database instance</param>
    public RealEstateObjectRepository(IMongoDatabase database)
        : base(database, "RealEstateObjects")
    {
    }
}