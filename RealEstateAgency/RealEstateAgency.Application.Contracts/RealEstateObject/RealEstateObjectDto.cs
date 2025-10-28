using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Data transfer object representing a <see cref="RealEstateObject"/> entity
/// Contains cadastral number, address, property characteristics, and purpose information
/// </summary>
public record RealEstateObjectDto(
    int Id, 
    string CadastralNumber, 
    string Address, 
    int Floors, 
    double TotalArea,
    int Rooms,
    double CeilingHeight,
    int FloorNumber,
    bool HasEncumbrance,
    PropertyType Type,
    PropertyPurpose Purpose);