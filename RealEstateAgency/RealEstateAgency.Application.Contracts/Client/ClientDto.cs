namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Data transfer object representing a <see cref="Client"/> entity
/// Contains personal information such as full name, passport number, and phone number
/// </summary>
public record ClientDto(
    int Id, 
    string FullName, 
    string PassportNumber, 
    string Phone);