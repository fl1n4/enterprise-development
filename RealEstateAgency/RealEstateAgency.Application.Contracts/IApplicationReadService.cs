namespace RealEstateAgency.Application.Contracts;

/// <summary>
/// Application service interface for performing read operations
/// </summary>
/// <typeparam name="TDto">DTO type used for GET requests</typeparam>
/// <typeparam name="TKey">Type of the DTO identifier</typeparam>
public interface IApplicationReadService<TDto, TKey>
    where TDto : class
    where TKey : struct
{
    /// <summary>
    /// Retrieves a DTO by its identifier
    /// </summary>
    /// <param name="dtoId">The DTO identifier</param>
    /// <returns>The found DTO</returns>
    public Task<TDto> Get(TKey dtoId);

    /// <summary>
    /// Retrieves all DTOs.
    /// </summary>
    /// <returns>A list of all DTOs</returns>
    public Task<IList<TDto>> GetAll();
}