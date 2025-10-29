namespace RealEstateAgency.Application.Contracts.Client;

/// <summary>
/// DTO representing a client entity.
/// </summary>
/// <param name="Id">Unique identifier of the client.</param>
/// <param name="FullName">Full name of the client.</param>
/// <param name="PassportNumber">Passport number used as a unique identifier.</param>
/// <param name="Phone">Contact phone number of the client.</param>
public record ClientDto(
    int Id, 
    string FullName, 
    string PassportNumber, 
    string Phone
);