//using RealEstateAgency.Application.Contracts;

namespace RealEstateAgency.Application.Contracts.RealEstateObject;

/// <summary>
/// Service interface for full CRUD operations on real estate objects.
/// </summary>
public interface IRealEstateObjectCRUDService : IApplicationReadService<RealEstateObjectDto, int>
{
    public Task<RealEstateObjectDto> Create(RealEstateObjectCreateUpdateDto dto);
    public Task<RealEstateObjectDto> Update(int id, RealEstateObjectCreateUpdateDto dto);
    public Task Delete(int id);
}