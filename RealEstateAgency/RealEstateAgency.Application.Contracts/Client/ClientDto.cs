namespace RealEstateAgency.Application.Contracts.Client;
public record ClientDto(
    int Id, 
    string FullName, 
    string PassportNumber, 
    string Phone);