using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Entities;

/// <summary>
/// Represents a real estate object managed by the agency
/// </summary>
public class RealEstateObject
{
    /// <summary>
    /// Unique cadastral number of the property
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Full address of the property
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Number of floors in the building
    /// </summary>
    public required int Floors { get; set; }

    /// <summary>
    /// Total area of the property in square meters
    /// </summary>
    public required double TotalArea { get; set; }

    /// <summary>
    /// Number of rooms in the property
    /// </summary>
    public required int Rooms { get; set; }

    /// <summary>
    /// Height of ceilings in meters.
    /// </summary>
    public required double CeilingHeight { get; set; }

    /// <summary>
    /// Floor number where the property is located
    /// </summary>
    public required int FloorNumber { get; set; }

    /// <summary>
    /// Indicates whether the property has any encumbrances or legal restrictions
    /// </summary>
    public required bool HasEncumbrance { get; set; }

    /// <summary>
    /// Type of the property (apartment, house, office, warehouse, land plot)
    /// </summary>
    public required PropertyType Type { get; set; }

    /// <summary>
    /// Purpose of the property (residential or commercial)
    /// </summary>
    public required PropertyPurpose Purpose { get; set; }
}
