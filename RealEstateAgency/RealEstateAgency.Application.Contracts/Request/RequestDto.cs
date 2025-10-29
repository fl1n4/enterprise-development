using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// DTO representing a real estate request.
/// </summary>
/// <param name="Id">Unique identifier of the request.</param>
/// <param name="Client">Client associated with the request.</param>
/// <param name="Property">Property associated with the request.</param>
/// <param name="Type">Type of the request (Buy or Sell).</param>
/// <param name="Amount">Requested amount in the transaction.</param>
/// <param name="DateCreated">Date when the request was created.</param>
public record RequestDto(
    int Id,
    ClientDto Client,
    RealEstateObjectDto Property,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated
);