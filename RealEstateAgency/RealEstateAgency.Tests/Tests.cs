using RealEstateAgency.Domain.Data;
using RealEstateAgency.Domain.Entities;
using RealEstateAgency.Domain.Enums;
using Xunit;

namespace RealEstateAgency.Tests;

/// <summary>
/// Набор тестов, проверяющих корректность LINQ-запросов к данным агентства
/// </summary>
public class RealEstateQueriesTests(RealEstateSeed fixture) : IClassFixture<RealEstateSeed>
{
    private readonly RealEstateSeed _fixture;    

    /// <summary>
    ///  Вывести всех продавцов, оставивших заявки за заданный период
    /// </summary>
    [Fact]
    public void GetSellersInPeriod_ShouldReturnCorrectClients()
    {
        // Arrange
        var requests = _fixture.Seed.Requests;
        var start = new DateTime(2025, 9, 15);
        var end = new DateTime(2025, 9, 30);

        // Act
        var sellers = requests
            .Where(r => r.Type == RequestType.Sell && r.DateCreated >= start && r.DateCreated <= end)
            .Select(r => r.Client.FullName)
            .Distinct()
            .ToList();

        // Assert
        Assert.NotEmpty(sellers);
        Assert.All(sellers, name => Assert.False(string.IsNullOrWhiteSpace(name)));
    }

    /// <summary>
    ///  Вывести топ 5 клиентов по количеству заявок (отдельно на покупку и продажу)
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRequestType_ShouldReturnTopClients()
    {
        var requests = fixture.Seed.Requests;

        var topBuyers = requests
            .Where(r => r.Type == RequestType.Buy)
            .GroupBy(r => r.Client.FullName)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var topSellers = requests
            .Where(r => r.Type == RequestType.Sell)
            .GroupBy(r => r.Client.FullName)
            .Select(g => new { Client = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.NotEmpty(topBuyers);
        Assert.NotEmpty(topSellers);
        Assert.True(topBuyers.Count <= 5);
        Assert.True(topSellers.Count <= 5);
    }

    /// <summary>
    /// Вывести информацию о количестве заявок по каждому типу недвижимости
    /// </summary>
    [Fact]
    public void GetRequestCountByPropertyType_ShouldReturnGroupedData()
    {
        var requests = _fixture.Seed.Requests;

        var stats = requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToList();

        Assert.NotEmpty(stats);
        Assert.All(stats, s => Assert.True(s.Count > 0));
    }

    /// <summary>
    /// Вывести информацию о клиентах, открывших заявки с минимальной стоимостью
    /// </summary>
    [Fact]
    public void GetClientsWithMinRequestAmount_ShouldReturnCorrectClients()
    {
        var requests = _fixture.Seed.Requests;
        var minAmount = requests.Min(r => r.Amount);

        var clientsWithMin = requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client)
            .Distinct()
            .ToList();

        Assert.NotEmpty(clientsWithMin);
        Assert.All(clientsWithMin, c => Assert.False(string.IsNullOrWhiteSpace(c.FullName)));
    }

    /// <summary>
    /// Вывести сведения о клиентах, ищущих недвижимость заданного типа, упорядочить по ФИО
    /// </summary>
    [Fact]
    public void GetClientsByPropertyType_ShouldReturnOrderedList()
    {
        var requests = _fixture.Seed.Requests;
        var type = PropertyType.Apartment;

        var clients = requests
            .Where(r => r.Property.Type == type && r.Type == RequestType.Buy)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.NotNull(clients);
        Assert.All(clients, c => Assert.False(string.IsNullOrWhiteSpace(c.FullName)));
        if (clients.Count > 1)
        {
            var ordered = clients.OrderBy(c => c.FullName).ToList();
            Assert.Equal(ordered, clients);
    }
}
