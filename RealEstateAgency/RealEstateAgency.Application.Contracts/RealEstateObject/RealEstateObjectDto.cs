using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// DTO representing a real estate object.
/// </summary>
/// <param name="Id">Unique identifier of the property.</param>
/// <param name="CadastralNumber">Unique cadastral number of the property.</param>
/// <param name="Address">Physical address of the property.</param>
/// <param name="Floors">Total number of floors in the building.</param>
/// <param name="TotalArea">Total area of the property in square meters.</param>
/// <param name="Rooms">Number of rooms in the property.</param>
/// <param name="CeilingHeight">Ceiling height in meters.</param>
/// <param name="FloorNumber">The floor number where the property is located.</param>
/// <param name="HasEncumbrance">Indicates whether the property has encumbrances.</param>
/// <param name="Type">Type of the property (e.g., apartment, house, land).</param>
/// <param name="Purpose">Purpose of the property (e.g., residential, commercial).</param>
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