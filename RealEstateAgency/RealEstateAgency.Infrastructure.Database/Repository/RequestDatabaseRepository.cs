using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RealEstateAgency.Infrastructure.Database.Repository;

public class RequestDatabaseRepository(RealEstateAgencyDbContext context) : IRepository<Request, int>
{
    public async Task<Request> Create(Request entity)
    {
        var result = await context.Requests.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Requests.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;

        context.Requests.Remove(entity);
        await context.SaveChangesAsync();

        return true;
    }
    public async Task<Request?> Get(int entityId) =>
        await context.Requests.FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Request>> GetAll() =>
        await context.Requests.ToListAsync();
    public async Task<Request> Update(Request entity)
    {
        context.Requests.Update(entity);

        await context.SaveChangesAsync();
        return entity;
    }
}
