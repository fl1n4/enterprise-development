using MongoDB.Driver;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Infrastructure.Mongo;

public class ClientRepository : MongoRepository<Client, int>, IClientRepository
{
    public ClientRepository(IMongoDatabase database)
        : base(database, "Clients")
    {
    }
}