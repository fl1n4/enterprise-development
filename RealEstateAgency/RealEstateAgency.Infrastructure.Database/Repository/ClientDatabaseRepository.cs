using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RealEstateAgency.Infrastructure.Database.Repository;

public class ClientDatabaseRepository(RealEstateAgencyDbContext context) : IRepository<Client, int>
{
    public async Task<Client> Create(Client entity)
    {
        var result = await context.Clients.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    public async Task<bool> Delete(int entityId)
    {
        var entity = await context.Clients.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null)
            return false;

        context.Clients.Remove(entity);
        await context.SaveChangesAsync();

        return true;
    }
    public async Task<Client?> Get(int entityId) =>
        await context.Clients.FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Client>> GetAll() =>
        await context.Clients.ToListAsync();
    public async Task<Client> Update(Client entity)
    {
        context.Clients.Update(entity);

        await context.SaveChangesAsync();
        return entity;
    }
}
