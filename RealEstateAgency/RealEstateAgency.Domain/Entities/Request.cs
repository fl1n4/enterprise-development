using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Entities;

/// <summary>
/// Represents a request submitted by a client to buy or sell a property
/// </summary>
public class Request
{
    /// <summary>
    /// Unique identifier of the request
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Identifier of the client who submitted the request
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// Identifier of the property associated with the request
    /// </summary>
    public required int PropertyId { get; set; }

    /// <summary>
    /// Type of the request (Buy or Sell)
    /// </summary>
    public RequestType? Type { get; set; }

    /// <summary>
    /// Monetary amount associated with the request
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Date when the request was created
    /// </summary>
    public DateOnly? DateCreated { get; set; }
}