namespace RealEstateAgency.Application.Contracts;

/// <summary>
/// Application service interface for performing CRUD operations
/// </summary>
/// <typeparam name="TDto">DTO type used for GET requests</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO type used for POST/PUT requests</typeparam>
/// <typeparam name="TKey">Type of the DTO identifier</typeparam>
public interface IApplicationCrudService<TDto, TCreateUpdateDto, TKey> : IApplicationReadService<TDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Creates a new DTO
    /// </summary>
    /// <param name="dto">DTO instance to create</param>
    /// <returns>The created DTO</returns>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Updates an existing DTO
    /// </summary>
    /// <param name="dto">The updated DTO data</param>
    /// <param name="dtoId">The identifier of the DTO to update</param>
    /// <returns>The updated DTO</returns>
    public Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);

    /// <summary>
    /// Deletes a DTO by its identifier
    /// </summary>
    /// <param name="dtoId">The DTO identifier</param>
    public Task<bool> Delete(TKey dtoId);
}