namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// DTO for creating or updating a client.
/// </summary>
public record ClientCreateUpdateDto(
    string FullName,
    string PassportNumber,
    string Phones
);