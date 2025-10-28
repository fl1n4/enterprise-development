using MongoDB.Driver;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Infrastructure.Mongo;

/// <summary>
/// Generic repository for managing entities in MongoDB
/// Provides asynchronous CRUD operations for entities identified by a numeric key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    protected readonly IMongoCollection<TEntity> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoRepository{TEntity, TKey}"/> class
    /// </summary>
    /// <param name="database">The MongoDB database instance</param>
    /// <param name="collectionName">The name of the MongoDB collection</param>
    protected MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    /// <summary>
    /// Creates a new entity in the collection
    /// </summary>
    /// <param name="entity">The entity to create</param>
    /// <returns>The created <typeparamref name="TEntity"/></returns>
    public async Task<TEntity> Create(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    /// <summary>
    /// Retrieves an entity by its identifier
    /// </summary>
    /// <param name="id">The entity identifier</param>
    /// <returns>The found entity or null if not found</returns>
    public async Task<TEntity?> Get(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves all entities from the collection
    /// </summary>
    /// <returns>A list of all entities</returns>
    public async Task<IList<TEntity>> GetAll()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Updates an existing entity in the collection
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <returns>The updated entity</returns>
    public async Task<TEntity> Update(TEntity entity)
    {
        var id = GetEntityId(entity);
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false });
        return entity;
    }

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="id">The identifier of the entity to delete</param>
    /// <returns>True if the entity was deleted, otherwise false</returns>
    public async Task<bool> Delete(TKey id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Gets the entity identifier value using reflection
    /// </summary>
    /// <param name="entity">The entity from which to get the identifier</param>
    /// <returns>The identifier value</returns>
    /// <exception cref="InvalidOperationException">Thrown if the entity does not have an Id property or it is null</exception>
    private static TKey GetEntityId(TEntity entity)
    {
        var idProperty = typeof(TEntity).GetProperty("Id")
            ?? throw new InvalidOperationException($"Entity {typeof(TEntity).Name} must have an 'Id' property.");

        var value = idProperty.GetValue(entity);
        if (value == null) throw new InvalidOperationException("Id cannot be null.");

        return (TKey)value;
    }
}