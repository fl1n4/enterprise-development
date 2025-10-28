using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Data transfer object representing a <see cref="Request"/> entity
/// Contains client information, related property, request type, amount, and creation date
/// </summary>
public record RequestDto(
    int Id,
    ClientDto Client,
    RealEstateObjectDto Property,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated);