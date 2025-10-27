using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// DTO for creating or updating a request.
/// </summary>
public record RequestCreateUpdateDto(
    int ClientId,
    int PropertyId,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated
);