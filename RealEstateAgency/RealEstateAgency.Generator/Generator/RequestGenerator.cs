using Bogus;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Enums;
namespace RealEstateAgency.Generator.Generator;

public static class RequestGenerator
{

    public static List<RequestCreateUpdateDto> GenerateRequests(int count) =>
        new Faker<RequestCreateUpdateDto>()
            .WithRecord()
            .RuleFor(dto => dto.ClientId, f => f.Random.Int(1, 100))
            .RuleFor(dto => dto.PropertyId, f => f.Random.Int(1, 100))
            .RuleFor(dto => dto.Type, f => f.PickRandom<RequestType>())
            .RuleFor(dto => dto.Amount, f => f.Random.Decimal(1000, 1000000))
            .RuleFor(dto => dto.DateCreated, f => f.Date.RecentDateOnly(30))
            .Generate(count);
}