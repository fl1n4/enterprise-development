using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// DTO for creating or updating a request.
/// </summary>
/// <param name="ClientId">Identifier of the client associated with the request.</param>
/// <param name="PropertyId">Identifier of the property associated with the request.</param>
/// <param name="Type">Type of the request (Buy or Sell).</param>
/// <param name="Amount">Requested amount in the transaction.</param>
/// <param name="DateCreated">Date when the request was created.</param>
public record RequestCreateUpdateDto(
    int ClientId,
    int PropertyId,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated
);