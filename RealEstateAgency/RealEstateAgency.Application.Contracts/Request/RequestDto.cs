namespace RealEstateAgency.Application.Contracts.Request;
public record RequestDto(
    int Id, 
    decimal Amount, 
    DateOnly DateCreated);
