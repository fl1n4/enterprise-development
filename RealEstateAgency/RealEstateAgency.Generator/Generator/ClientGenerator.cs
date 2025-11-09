using Bogus;
using RealEstateAgency.Application.Contracts.Client;
namespace RealEstateAgency.Generator.Generator;

public static class ClientGenerator
{

    public static List<ClientCreateUpdateDto> GenerateClients(int count) =>
        new Faker<ClientCreateUpdateDto>()
            .WithRecord()
            .RuleFor(dto => dto.FullName, f => f.Person.FullName)
            .RuleFor(dto => dto.PassportNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(dto => dto.Phone, f => f.Phone.PhoneNumber())
            .Generate(count);
}