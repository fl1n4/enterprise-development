using Bogus;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Enums;
namespace RealEstateAgency.Generator.Generator;

/// <summary>
/// Provides functionality for generating random request data for testing
/// </summary>
public static class RequestGenerator
{
    /// <summary>
    /// Generates a collection of random requests using predefined faker rules
    /// </summary>
    /// <param name="count">Number of requests to generate</param>
    /// <returns>List of randomly generated <see cref="RequestCreateUpdateDto"/> objects</returns>
    public static List<RequestCreateUpdateDto> GenerateRequests(int count) =>
        new Faker<RequestCreateUpdateDto>()
            .WithRecord()
            .RuleFor(dto => dto.ClientId, f => f.Random.Int(11, 20))
            .RuleFor(dto => dto.PropertyId, f => f.Random.Int(11, 20))
            .RuleFor(dto => dto.Type, f => f.PickRandom<RequestType>())
            .RuleFor(dto => dto.Amount, f => f.Random.Decimal(1000000, 10000000))
            .RuleFor(dto => dto.DateCreated, f => f.Date.RecentDateOnly(30))
            .Generate(count);
}