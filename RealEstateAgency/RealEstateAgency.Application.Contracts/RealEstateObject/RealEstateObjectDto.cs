namespace RealEstateAgency.Application.Contracts.RealEstateObject;
public record RealEstateObjectDto(
    int Id, 
    string CadastralNumber, 
    string Address, 
    int Floors, 
    double TotalArea,
    int Rooms,
    double CeilingHeight,
    int FloorNumber,
    bool HasEncumbrance);