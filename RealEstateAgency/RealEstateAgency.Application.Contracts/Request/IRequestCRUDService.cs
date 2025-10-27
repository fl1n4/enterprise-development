using RealEstateAgency.Application.Contracts;

namespace RealEstateAgency.Application.Contracts.Request;

/// <summary>
/// Service interface for full CRUD operations on requests.
/// </summary>
public interface IRequestCRUDService : IApplicationReadService<RequestDto, int>
{
    public Task<RequestDto> Create(RequestCreateUpdateDto dto);
    public Task<RequestDto> Update(int id, RequestCreateUpdateDto dto);
    public Task Delete(int id);
}