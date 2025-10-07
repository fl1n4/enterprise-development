using RealEstateAgency.Domain.Data;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

/// <summary>
/// Ќабор тестов дл€ проверки выборок из данных RealEstateSeed
/// </summary>
public class RealEstateQueriesTests(RealEstateSeed testData) : IClassFixture<RealEstateSeed>
{
    /// <summary>
    ///  ѕолучение всех продавцов, оставивших за€вки за заданный период
    /// </summary>
    [Fact]
    public void GetSellersByPeriod()
    {
        // arrange
        var from = new DateTime(2025, 9, 17);
        var to = new DateTime(2025, 9, 26);

        // act
        var sellers = testData.Requests
            .Where(r => r.Type == RequestType.Sell && r.DateCreated >= from && r.DateCreated <= to)
            .Select(r => r.Client.FullName)
            .Distinct()
            .ToList();

        // assert
        Assert.NotEmpty(sellers);
        Assert.All(sellers, s => Assert.False(string.IsNullOrWhiteSpace(s)));
    }

    /// <summary>
    ///  ѕолучение топ-5 клиентов по количеству за€вок (на покупку)
    /// </summary>
    [Fact]
    public void GetTop5BuyersByRequests()
    {
        var topBuyers = testData.Requests
            .Where(r => r.Type == RequestType.Buy)
            .GroupBy(r => r.Client.FullName)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.NotEmpty(topBuyers);
        Assert.True(topBuyers.Count <= 5);
    }

    /// <summary>
    ///  ѕолучение топ-5 клиентов по количеству за€вок (на продажу)
    /// </summary>
    [Fact]
    public void GetTop5SellersByRequests()
    {
        var topSellers = testData.Requests
            .Where(r => r.Type == RequestType.Sell)
            .GroupBy(r => r.Client.FullName)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.NotEmpty(topSellers);
        Assert.True(topSellers.Count <= 5);
    }

    /// <summary>
    /// ѕолучение количества за€вок по каждому типу недвижимости
    /// </summary>
    [Fact]
    public void GetRequestCountByPropertyType()
    {
        var counts = testData.Requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new { PropertyType = g.Key, Count = g.Count() })
            .ToList();

        Assert.NotEmpty(counts);
        Assert.All(counts, x => Assert.True(x.Count > 0));
    }

    /// <summary>
    /// ѕолучение клиентов с минимальной стоимостью за€вки
    /// </summary>
    [Fact]
    public void GetClientsWithMinRequestAmount()
    {
        var minAmount = testData.Requests.Min(r => r.Amount);
        var clients = testData.Requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client.FullName)
            .Distinct()
            .ToList();

        Assert.NotEmpty(clients);
    }

    /// <summary>
    /// ѕолучение клиентов, ищущих недвижимость заданного типа (например, квартиры)
    /// </summary>
    [Fact]
    public void GetClientsByPropertyType()
    {
        var targetType = PropertyType.Apartment;

        var clients = testData.Requests
            .Where(r => r.Property.Type == targetType && r.Type == RequestType.Buy)
            .Select(r => r.Client.FullName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        Assert.NotEmpty(clients);
        Assert.Equal(clients.OrderBy(n => n), clients);
    }
}
