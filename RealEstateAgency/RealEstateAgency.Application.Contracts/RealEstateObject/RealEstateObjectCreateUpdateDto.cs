using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// DTO for creating or updating a real estate object.
/// </summary>
public record RealEstateObjectCreateUpdateDto(
    string CadastralNumber,
    string Address,
    int Floors,
    double TotalArea,
    int Rooms,
    double CeilingHeight,
    int FloorNumber,
    bool HasEncumbrance,
    PropertyType Type,
    PropertyPurpose Purpose
);