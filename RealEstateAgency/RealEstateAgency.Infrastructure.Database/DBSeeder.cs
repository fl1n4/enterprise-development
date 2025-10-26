using RealEstateAgency.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace RealEstateAgency.Infrastructure.Database;


/// <summary>
/// Seeds the database with initial data using <see cref="DataSeeder"/>.
/// </summary>
public class DbSeeder(RealEstateAgencyDbContext context)
{
    /// <summary>
    /// Seeds the database asynchronously with predefined data.
    /// </summary>
    public async Task Seed()
    {
        var seed = new RealEstateSeed();

        var anyData =
            await context.Clients.AnyAsync() ||
            await context.RealEstateObjects.AnyAsync() ||
            await context.Requests.AnyAsync();

        if (anyData)
            return;

        await context.Clients.AddRangeAsync(seed.Clients);
        await context.SaveChangesAsync();

        await context.RealEstateObjects.AddRangeAsync(seed.Properties);
        await context.SaveChangesAsync();

        await context.Requests.AddRangeAsync(seed.Requests);
        await context.SaveChangesAsync();
    }
}