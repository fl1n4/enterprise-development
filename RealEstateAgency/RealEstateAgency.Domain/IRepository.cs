namespace RealEstateAgency.Domain;

/// <summary>
/// Defines a generic repository interface that provides 
/// basic CRUD (Create, Read, Update, Delete) operations
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity being managed by the repository
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the entity's unique identifier (e.g., <see cref="int"/>).
/// Must be a value type (<see langword="struct"/>)
/// </typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Adds a new entity to the repository
    /// </summary>
    /// <param name="entity">The entity instance to add</param>
    public Task<TEntity> Create(TEntity entity);

    /// <summary>
    /// Retrieves an entity from the repository by its identifier
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity</param>
    /// <returns>
    /// The entity with the specified identifier, or <see langword="null"/> 
    /// if no entity with such an identifier exists
    /// </returns>
    public Task<TEntity?> Get(TKey entityId);

    /// <summary>
    /// Retrieves all entities stored in the repository
    /// </summary>
    /// <returns>
    /// A list containing all entities in the repository
    /// </returns>
    public Task<IList<TEntity>> GetAll();

    /// <summary>
    /// Updates an existing entity in the repository
    /// </summary>
    /// <param name="entity">The entity instance containing updated data</param>
    public Task<TEntity> Update(TEntity entity);

    /// <summary>
    /// Removes an entity from the repository by its identifier
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity to delete</param>
    public Task<bool> Delete(TKey entityId);
}
