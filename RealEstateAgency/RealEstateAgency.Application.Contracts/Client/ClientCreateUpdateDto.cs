namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// DTO for creating or updating a client.
/// </summary>
/// <param name="FullName">Full name of the client.</param>
/// <param name="PassportNumber">Passport number used as a unique identifier.</param>
/// <param name="Phone">Contact phone number of the client.</param>
public record ClientCreateUpdateDto(
    string FullName,
    string PassportNumber,
    string Phone
);