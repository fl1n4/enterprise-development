using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Data transfer object for creating or updating <see cref="RealEstateObject"/> entities
/// Includes cadastral number, address, property characteristics, and purpose information
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