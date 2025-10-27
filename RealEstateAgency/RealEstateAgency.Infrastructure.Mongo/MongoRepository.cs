using MongoDB.Driver;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Infrastructure.Mongo;

public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<TEntity?> Get(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IList<TEntity>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        var id = GetEntityId(entity);
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false });
        return entity;
    }

    public async Task<bool> Delete(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    private static TKey GetEntityId(TEntity entity)
    {
        var idProperty = typeof(TEntity).GetProperty("Id")
            ?? throw new InvalidOperationException($"Entity {typeof(TEntity).Name} must have an 'Id' property.");

        var value = idProperty.GetValue(entity);
        if (value == null) throw new InvalidOperationException("Id cannot be null.");

        return (TKey)value;
    }
}