using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Data;

/// <summary>
/// Тестовый набор данных для RealEstateAgency
/// Используется в фикстурах и юнит-тестах
/// </summary>
public class RealEstateSeed
{
    public List<Client> Clients { get; } = new();
    public List<RealEstateObject> Properties { get; } = new();
    public List<Request> Requests { get; } = new();

    public RealEstateSeed()
    {
        Clients.AddRange(new[]
        {
            new Client { FullName = "Иванов Иван Иванович", 
                PassportNumber = "4000 123456", 
                Phone = "+79001112233" },
            new Client { FullName = "Петрова Анна Сергеевна", 
                PassportNumber = "4001 654321", 
                Phone = "+79002223344" },
            new Client { FullName = "Сидоров Павел Дмитриевич", 
                PassportNumber = "4002 987654", 
                Phone = "+79003334455" },
            new Client { FullName = "Кузнецова Мария Александровна", 
                PassportNumber = "4003 456789", 
                Phone = "+79004445566" },
            new Client { FullName = "Смирнов Алексей Игоревич", 
                PassportNumber = "4004 321987", 
                Phone = "+79005556677" }
        });

        Properties.AddRange(new[]
        {
            new RealEstateObject
            {
                CadastralNumber = "77:01:0004012:1234",
                Address = "г. Москва, ул. Ленина, д. 10",
                Floors = 10,
                TotalArea = 75.5,
                Rooms = 3,
                CeilingHeight = 2.7,
                FloorNumber = 5,
                HasEncumbrance = false,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential
            },
            new RealEstateObject
            {
                CadastralNumber = "77:02:0001111:5678",
                Address = "г. Москва, пр-т Мира, д. 45",
                Floors = 16,
                TotalArea = 120.0,
                Rooms = 4,
                CeilingHeight = 3.0,
                FloorNumber = 10,
                HasEncumbrance = false,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential
            },
            new RealEstateObject
            {
                CadastralNumber = "78:04:0002222:2222",
                Address = "г. Санкт-Петербург, Невский пр-т, д. 100",
                Floors = 5,
                TotalArea = 250.0,
                Rooms = 10,
                CeilingHeight = 3.2,
                FloorNumber = 1,
                HasEncumbrance = true,
                Type = PropertyType.Office,
                Purpose = PropertyPurpose.Commercial
            },
            new RealEstateObject
            {
                CadastralNumber = "77:07:0009999:3333",
                Address = "г. Москва, ул. Строителей, д. 7",
                Floors = 2,
                TotalArea = 180.0,
                Rooms = 6,
                CeilingHeight = 2.9,
                FloorNumber = 2,
                HasEncumbrance = false,
                Type = PropertyType.House,
                Purpose = PropertyPurpose.Residential
            },
            new RealEstateObject
            {
                CadastralNumber = "50:10:0011222:4444",
                Address = "МО, г. Химки, ул. Кирова, д. 3",
                Floors = 1,
                TotalArea = 1000.0,
                Rooms = 1,
                CeilingHeight = 4.5,
                FloorNumber = 1,
                HasEncumbrance = true,
                Type = PropertyType.Warehouse,
                Purpose = PropertyPurpose.Commercial
            }
        });

        Requests.AddRange(new[]
        {
            new Request
            {
                Client = Clients[0],
                Property = Properties[0],
                Type = RequestType.Buy,
                Amount = 12_500_000,
                DateCreated = new DateTime(2025, 9, 15)
            },
            new Request
            {
                Client = Clients[1],
                Property = Properties[2],
                Type = RequestType.Sell,
                Amount = 25_000_000,
                DateCreated = new DateTime(2025, 9, 18)
            },
            new Request
            {
                Client = Clients[2],
                Property = Properties[1],
                Type = RequestType.Buy,
                Amount = 14_000_000,
                DateCreated = new DateTime(2025, 9, 20)
            },
            new Request
            {
                Client = Clients[3],
                Property = Properties[3],
                Type = RequestType.Sell,
                Amount = 19_000_000,
                DateCreated = new DateTime(2025, 9, 22)
            },
            new Request
            {
                Client = Clients[4],
                Property = Properties[4],
                Type = RequestType.Sell,
                Amount = 30_000_000,
                DateCreated = new DateTime(2025, 9, 25)
            }
        });
    }
}
