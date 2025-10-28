namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// Data transfer object for creating or updating <see cref="Client"/> entities
/// Includes personal information such as full name, passport number, and phone number
/// </summary>
public record ClientCreateUpdateDto(
    string FullName,
    string PassportNumber,
    string Phone
);