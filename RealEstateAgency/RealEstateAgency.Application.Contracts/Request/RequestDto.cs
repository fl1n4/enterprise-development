using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain.Enums;
namespace RealEstateAgency.Application.Contracts.Request;
public record RequestDto(
    int Id,
    ClientDto Client,
    RealEstateObjectDto Property,
    RequestType? Type,
    decimal? Amount,
    DateOnly? DateCreated);