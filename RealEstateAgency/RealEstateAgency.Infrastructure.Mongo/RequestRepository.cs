using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Infrastructure.Mongo;

public class RequestRepository : MongoRepository<Request, int>, IRequestRepository
{
    public RequestRepository(IMongoDatabase database)
        : base(database, "Requests")
    {
    }
}