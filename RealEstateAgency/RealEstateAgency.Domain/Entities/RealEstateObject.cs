using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Entities;
public class RealEstateObject
{
    public required string CadastralNumber { get; set; }
    public required string Address { get; set; }
    public required int Floors { get; set; }
    public required double TotalArea { get; set; }
    public required int Rooms { get; set; }
    public required double CeilingHeight { get; set; }
    public required int FloorNumber { get; set; }
    public required bool HasEncumbrance { get; set; }
    public required PropertyType Type { get; set; }
    public required PropertyPurpose Purpose { get; set; }
}
