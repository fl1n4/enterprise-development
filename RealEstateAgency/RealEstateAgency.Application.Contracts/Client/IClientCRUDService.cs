using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Interface defining CRUD operations for managing <see cref="ClientDto"/> entities
/// Uses <see cref="ClientCreateUpdateDto"/> for create and update operations
/// </summary>
public interface IClientCRUDService : IApplicationCrudService<ClientDto, ClientCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all clients who acted as sellers within the specified date range.
    /// </summary>
    /// <param name="from">Start date of the period.</param>
    /// <param name="to">End date of the period.</param>
    /// <returns>A list of <see cref="ClientDto"/> representing sellers in the specified period.</returns>
    public Task<IList<ClientDto>> GetSellersByPeriod(DateOnly from, DateOnly to);

    /// <summary>
    /// Retrieves the top 5 clients for each request type (Buy/Sell), ranked by the number of requests.
    /// </summary>
    /// <returns>
    /// A dictionary where each key is a <see cref="RequestType"/>, 
    /// and each value is a list of up to 5 <see cref="ClientDto"/> objects.
    /// </returns>
    public Task<Dictionary<RequestType, List<ClientDto>>> GetTop5ClientsByRequestType();

    /// <summary>
    /// Retrieves clients who have requests with the minimum amount value.
    /// </summary>
    /// <returns>A list of <see cref="ClientDto"/> representing clients with the lowest request amount.</returns>
    public Task<IList<ClientDto>> GetClientsWithMinRequestAmount();

    /// <summary>
    /// Retrieves clients interested in buying properties of a specified type,
    /// sorted alphabetically by their full names.
    /// </summary>
    /// <param name="type">The property type to filter clients by.</param>
    /// <returns>A list of <see cref="ClientDto"/> representing clients interested in that property type.</returns>
    public Task<IList<ClientDto>> GetClientsByPropertyType(PropertyType type);
}