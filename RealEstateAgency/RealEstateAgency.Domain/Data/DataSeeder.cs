using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Domain.Data;

/// <summary>
/// Represents a seeded dataset for testing and fixture purposes in the RealEstateAgency project
/// Contains collections of clients, properties, and requests
/// </summary>
public class RealEstateSeed
{
    /// <summary>
    /// Collection of clients in the seeded dataset
    /// </summary>
    public List<Client> Clients { get; } = new();

    /// <summary>
    /// Collection of real estate objects in the seeded dataset
    /// </summary>
    public List<RealEstateObject> Properties { get; } = new();

    /// <summary>
    /// Collection of requests made by clients for properties
    /// </summary>
    public List<Request> Requests { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RealEstateSeed"/> class with predefined data
    /// </summary>
    public RealEstateSeed()
    {
        Clients.AddRange(new[]
        {
            new Client 
            { 
                FullName = "Иванов Иван Иванович",
                PassportNumber = "4000 123456",
                Phone = "79001112233" 
            },
            new Client 
            { 
                FullName = "Петрова Анна Сергеевна",
                PassportNumber = "4001 654321",
                Phone = "79002223344" 
            },
            new Client { 
                FullName = "Сидоров Павел Дмитриевич",
                PassportNumber = "4002 987654",
                Phone = "79003334455" 
            },
            new Client 
            { 
                FullName = "Кузнецова Мария Александровна",
                PassportNumber = "4003 456789",
                Phone = "79004445566" 
            },
            new Client 
            { 
                FullName = "Смирнов Алексей Игоревич",
                PassportNumber = "4004 321987",
                Phone = "79005556677" 
            },
            new Client 
            { 
                FullName = "Васильев Николай Петрович",
                PassportNumber = "4005 789123",
                Phone = "79006667788" 
            },
            new Client 
            { 
                FullName = "Морозова Екатерина Олеговна",
                PassportNumber = "4006 147258",
                Phone = "79007778899" 
            },
            new Client 
            { 
                FullName = "Громов Артём Сергеевич",
                PassportNumber = "4007 369258",
                Phone = "79008889900" 
            },
            new Client 
            { 
                FullName = "Лебедева Дарья Андреевна",
                PassportNumber = "4008 951753",
                Phone = "79009990011" 
            },
            new Client 
            { 
                FullName = "Попов Дмитрий Валерьевич",
                PassportNumber = "4009 753159",
                Phone = "79001002030" 
            }
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
            },
            new RealEstateObject
            {
                CadastralNumber = "77:03:0005555:5555",
                Address = "г. Москва, ул. Пушкина, д. 25",
                Floors = 9,
                TotalArea = 65.0,
                Rooms = 2,
                CeilingHeight = 2.6,
                FloorNumber = 3,
                HasEncumbrance = false,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential
            },
            new RealEstateObject
            {
                CadastralNumber = "78:06:0008888:6666",
                Address = "г. Санкт-Петербург, Литейный пр-т, д. 12",
                Floors = 8,
                TotalArea = 300.0,
                Rooms = 8,
                CeilingHeight = 3.1,
                FloorNumber = 2,
                HasEncumbrance = false,
                Type = PropertyType.Office,
                Purpose = PropertyPurpose.Commercial
            },
            new RealEstateObject
            {
                CadastralNumber = "77:09:0003333:7777",
                Address = "г. Москва, ул. Новая, д. 3",
                Floors = 3,
                TotalArea = 200.0,
                Rooms = 5,
                CeilingHeight = 3.0,
                FloorNumber = 1,
                HasEncumbrance = true,
                Type = PropertyType.House,
                Purpose = PropertyPurpose.Residential
            },
            new RealEstateObject
            {
                CadastralNumber = "50:20:0007777:8888",
                Address = "МО, г. Одинцово, ул. Гагарина, д. 8",
                Floors = 1,
                TotalArea = 1500.0,
                Rooms = 1,
                CeilingHeight = 5.0,
                FloorNumber = 1,
                HasEncumbrance = false,
                Type = PropertyType.Warehouse,
                Purpose = PropertyPurpose.Commercial
            },
            new RealEstateObject
            {
                CadastralNumber = "77:12:0006666:9999",
                Address = "г. Москва, пр-т Вернадского, д. 90",
                Floors = 25,
                TotalArea = 95.0,
                Rooms = 3,
                CeilingHeight = 2.8,
                FloorNumber = 20,
                HasEncumbrance = false,
                Type = PropertyType.Apartment,
                Purpose = PropertyPurpose.Residential
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
                DateCreated = new DateOnly(2025, 9, 15)
            },
            new Request
            {
                Client = Clients[1],
                Property = Properties[2],
                Type = RequestType.Sell,
                Amount = 25_000_000,
                DateCreated = new DateOnly(2025, 9, 18)
            },
            new Request
            {
                Client = Clients[2],
                Property = Properties[1],
                Type = RequestType.Buy,
                Amount = 14_000_000,
                DateCreated = new DateOnly(2025, 9, 20)
            },
            new Request
            {
                Client = Clients[3],
                Property = Properties[3],
                Type = RequestType.Sell,
                Amount = 19_000_000,
                DateCreated = new DateOnly(2025, 9, 22)
            },
            new Request
            {
                Client = Clients[4],
                Property = Properties[4],
                Type = RequestType.Sell,
                Amount = 30_000_000,
                DateCreated = new DateOnly(2025, 9, 25)
            },
            new Request
            {
                Client = Clients[5],
                Property = Properties[5],
                Type = RequestType.Buy,
                Amount = 9_500_000,
                DateCreated = new DateOnly(2025, 9, 26)
            },
            new Request
            {
                Client = Clients[6],
                Property = Properties[6],
                Type = RequestType.Sell,
                Amount = 22_000_000,
                DateCreated = new DateOnly(2025, 9, 27)
            },
            new Request
            {
                Client = Clients[7],
                Property = Properties[7],
                Type = RequestType.Buy,
                Amount = 16_000_000,
                DateCreated = new DateOnly(2025, 9, 28)
            },
            new Request
            {
                Client = Clients[8],
                Property = Properties[8],
                Type = RequestType.Sell,
                Amount = 28_000_000,
                DateCreated = new DateOnly(2025, 9, 29)
            },
            new Request
            {
                Client = Clients[9],
                Property = Properties[9],
                Type = RequestType.Buy,
                Amount = 11_000_000,
                DateCreated = new DateOnly(2025, 9, 30)
            }
        });
    }
}