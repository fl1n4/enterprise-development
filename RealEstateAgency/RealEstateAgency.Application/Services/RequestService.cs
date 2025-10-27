// RealEstateAgency.Application.Services/RequestService.cs
using MapsterMapper;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain;
using RealEstateAgency.Domain.Entities;

namespace RealEstateAgency.Application.Services;

/// <summary>
/// Service providing full CRUD operations for <see cref="Request"/> entities.
/// Implements <see cref="IRequestCRUDService"/>.
/// </summary>
public class RequestService(
    IRequestRepository repository,
    IMapper mapper)
    : IRequestCRUDService
{
    private static int _nextId = 1;

    public async Task<RequestDto> Create(RequestCreateUpdateDto dto)
    {
        var entity = mapper.Map<Request>(dto);
        entity.Id = Interlocked.Increment(ref _nextId);
        var created = await repository.Create(entity);
        return mapper.Map<RequestDto>(created);
    }

    public async Task<RequestDto> Update(RequestCreateUpdateDto dto, int dtoId)
    {
        var existing = await repository.Get(dtoId)
                      ?? throw new KeyNotFoundException($"Request with ID {dtoId} not found");
        var updatedEntity = mapper.Map(dto, existing);
        var updated = await repository.Update(updatedEntity);
        return mapper.Map<RequestDto>(updated);
    }

    public async Task<bool> Delete(int dtoId)
    {
        return await repository.Delete(dtoId);
    }

    public async Task<RequestDto> Get(int dtoId)
    {
        var entity = await repository.Get(dtoId)
                     ?? throw new KeyNotFoundException($"Request with ID {dtoId} not found");
        return mapper.Map<RequestDto>(entity);
    }

    public async Task<IList<RequestDto>> GetAll() =>
        mapper.Map<List<RequestDto>>(await repository.GetAll());
}