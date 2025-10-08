namespace RealEstateAgency.Domain.Entities;

/// <summary>
/// Represents a client of the real estate agency
/// </summary>
public class Client
{
    /// <summary>
    /// Unique identifier of the client
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the client
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Passport number of the client, used as a unique identifier
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Contact phone number of the client
    /// </summary>
    public required string Phone { get; set; }
}