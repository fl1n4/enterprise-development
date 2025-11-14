using Bogus;
using RealEstateAgency.Application.Contracts.Client;

namespace RealEstateAgency.Generator.RabbitMq.Host.Generator;

/// <summary>
/// Provides functionality for generating random client data for testing
/// </summary>
public static class ClientGenerator
{
    /// <summary>
    /// Generates a collection of random clients using predefined faker rules
    /// </summary>
    /// <param name="count">Number of clients to generate</param>
    /// <returns>List of randomly generated <see cref="ClientCreateUpdateDto"/> objects</returns>
    public static List<ClientCreateUpdateDto> GenerateClients(int count) =>
        new Faker<ClientCreateUpdateDto>()
            .WithRecord()
            .RuleFor(dto => dto.FullName, f => f.Person.FullName)
            .RuleFor(dto => dto.PassportNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(dto => dto.Phone, f => $"79{f.Random.Int(100000000, 999999999)}")
            .Generate(count);
}