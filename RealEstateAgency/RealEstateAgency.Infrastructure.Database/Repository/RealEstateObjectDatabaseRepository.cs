using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RealEstateAgency.Infrastructure.Database.Repository;

public class RealEstateObjectDatabaseRepository(RealEstateAgencyDbContext context) : IRepository<RealEstateObject, int>
{
    public async Task<RealEstateObject> Create(RealEstateObject entity)
    {
        var result = await context.RealEstateObjects.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.RealEstateObjects.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;

        context.RealEstateObjects.Remove(entity);
        await context.SaveChangesAsync();

        return true;
    }
    public async Task<RealEstateObject?> Get(int entityId) =>
        await context.RealEstateObjects.FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<RealEstateObject>> GetAll() =>
        await context.RealEstateObjects.ToListAsync();
    public async Task<RealEstateObject> Update(RealEstateObject entity)
    {
        context.RealEstateObjects.Update(entity);

        await context.SaveChangesAsync();
        return entity;
    }
}
