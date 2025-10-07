using RealEstateAgency.Domain.Data;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Tests;

/// <summary>
/// ����� ������ ��� �������� ������� �� ������ RealEstateSeed
/// </summary>
public class RealEstateQueriesTests(RealEstateSeed testData) : IClassFixture<RealEstateSeed>
{
    /// <summary>
    ///  ��������� ���� ���������, ���������� ������ �� �������� ������
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
    ///  ��������� ���-5 �������� �� ���������� ������ (�� �������)
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
    ///  ��������� ���-5 �������� �� ���������� ������ (�� �������)
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
    /// ��������� ���������� ������ �� ������� ���� ������������
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
    /// ��������� �������� � ����������� ���������� ������
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
    /// ��������� ��������, ������ ������������ ��������� ���� (��������, ��������)
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
