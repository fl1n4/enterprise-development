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
    /// Tests retrieval of top 5 buyers by number of buy requests
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
    /// Tests retrieval of top 5 sellers by number of sell requests
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
    /// Tests counting of requests grouped by property type
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
    /// Tests retrieval of clients who submitted requests with the minimum amount
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
    /// Tests retrieval of clients who submitted buy requests for a specified property type, ordered by full name
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
