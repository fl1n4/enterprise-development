using Bogus;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Domain.Enums;
namespace RealEstateAgency.Generator.Generator;

/// <summary>
/// Provides functionality for generating random real estate object data for testing
/// </summary>
public static class RealEstateObjectGenerator
{
    /// <summary>
    /// Generates a collection of random real estate objects using predefined faker rules
    /// </summary>
    /// <param name="count">Number of real estate objects to generate</param>
    /// <returns>List of randomly generated <see cref="RealEstateObjectCreateUpdateDto"/> objects</returns>
    public static List<RealEstateObjectCreateUpdateDto> GenerateRealEstateObjects(int count) =>
        new Faker<RealEstateObjectCreateUpdateDto>()
            .WithRecord()
            .RuleFor(dto => dto.CadastralNumber, f => f.Random.AlphaNumeric(12))
            .RuleFor(dto => dto.Address, f => f.Address.FullAddress())
            .RuleFor(dto => dto.Floors, f => f.Random.Int(1, 20))
            .RuleFor(dto => dto.TotalArea, f => f.Random.Double(20, 500))
            .RuleFor(dto => dto.Rooms, f => f.Random.Int(1, 6))
            .RuleFor(dto => dto.CeilingHeight, f => f.Random.Double(2.5, 4))
            .RuleFor(dto => dto.FloorNumber, f => f.Random.Int(1, 20))
            .RuleFor(dto => dto.HasEncumbrance, f => f.Random.Bool(0.1f))
            .RuleFor(dto => dto.Type, f => f.PickRandom<PropertyType>())
            .RuleFor(dto => dto.Purpose, f => f.PickRandom<PropertyPurpose>()) 
            .Generate(count);
}
