using RealEstateAgency.Domain.Data;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

/// <summary>
/// Unit tests for querying real estate data using <see cref="RealEstateSeed"/> fixture
/// </summary>
public class RealEstateQueriesTests(RealEstateSeed testData) : IClassFixture<RealEstateSeed>
{
    /// <summary>
    /// Tests retrieval of all sellers who submitted sell requests within a given period
    /// </summary>
    [Fact]
    public void GetSellersByPeriod_WhenDateInRange_ReturnsExpectedSellers()
    {
        var from = new DateOnly(2025, 9, 17);
        var to = new DateOnly(2025, 9, 26);

        var sellers = testData.Requests
            .Where(r => r.Type == RequestType.Sell
                        && r.DateCreated >= from
                        && r.DateCreated <= to)
            .Select(r => r.Client.FullName)
            .Distinct()
            .ToList();

        Assert.Equal(3, sellers.Count);
        Assert.Contains("Петрова Анна Сергеевна", sellers);
    }

    /// <summary>
    /// Tests retrieval of top 5 clients by request type (Buy and Sell)
    /// Verifies that each list contains at most five clients and is correctly grouped by type
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRequestType_WhenGroupedByBuyAndSell_ReturnsAtMostFivePerGroup()
    {
        var groupedTopClients = testData.Requests
            .Where(r => r.Type != null)
            .GroupBy(r => r.Type)
            .Select(g => new
            {
                Type = g.Key.ToString()!.ToLower(),
                Clients = g.GroupBy(r => r.Client.FullName)
                           .Select(cg => new { Client = cg.Key, Count = cg.Count() })
                           .OrderByDescending(x => x.Count)
                           .Take(5)
                           .Select(x => x.Client)
                           .ToList()
            })
            .ToList();

        Assert.NotNull(groupedTopClients);
        Assert.Equal(2, groupedTopClients.Count);

        foreach (var group in groupedTopClients)
        {
            Assert.True(group.Clients.Count <= 5, $"Too many clients in {group.Type}");
            Assert.NotEmpty(group.Clients);
        }

        var allClients = groupedTopClients.SelectMany(g => g.Clients).ToList();
        Assert.Contains("Иванов Иван Иванович", allClients);
        Assert.Contains("Петрова Анна Сергеевна", allClients);
    }

    /// <summary>
    /// Tests counting of requests grouped by property type
    /// </summary>
    [Fact]
    public void GetRequestCountByPropertyType_WhenGrouped_ReturnsCountForEachType()
    {
        var counts = testData.Requests
            .GroupBy(r => r.Property.Type)
            .Select(g => new { PropertyType = g.Key, Count = g.Count() })
            .ToList();

        Assert.NotEmpty(counts);
        Assert.All(counts, x => Assert.True(x.Count > 0));
    }

    /// <summary>
    /// Tests retrieval of clients who submitted requests with the minimum amount
    /// </summary>
    [Fact]
    public void GetClientsWithMinRequestAmount_WhenMinAmountExists_ReturnsExpectedClients()
    {
        var minAmount = testData.Requests.Min(r => r.Amount);

        var clients = testData.Requests
            .Where(r => r.Amount == minAmount)
            .Select(r => r.Client.FullName)
            .Distinct()
            .ToList();

        Assert.Single(clients);
        Assert.Contains("Васильев Николай Петрович", clients);
    }

    /// <summary>
    /// Tests retrieval of clients who submitted buy requests for a specified property type, ordered by full name
    /// </summary>
    [Fact]
    public void GetClientsByPropertyType_WhenFilteredByType_ReturnsAlphabeticallySortedClients()
    {
        var targetType = PropertyType.Apartment;

        var clients = testData.Requests
            .Where(r => r.Property.Type == targetType && r.Type == RequestType.Buy)
            .Select(r => r.Client.FullName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        Assert.NotNull(clients);
        Assert.NotEmpty(clients);

        var isOrdered = clients.SequenceEqual(clients.OrderBy(n => n));
        Assert.True(isOrdered, "The list of customers should be sorted by first name in alphabetical order");

        Assert.Contains("Иванов Иван Иванович", clients);
    }
}