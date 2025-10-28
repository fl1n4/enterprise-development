using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Infrastructure.Mongo;

public class RealEstateObjectRepository : MongoRepository<RealEstateObject, int>, IRealEstateObjectRepository
{
    public RealEstateObjectRepository(IMongoDatabase database)
        : base(database, "RealEstateObjects")
    {
    }
}