using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Data transfer object for creating or updating <see cref="Request"/> entities
/// Includes identifiers for client and property, request type, amount, and creation date
/// </summary>
public record RequestCreateUpdateDto(
    int ClientId,
    int PropertyId,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated
);