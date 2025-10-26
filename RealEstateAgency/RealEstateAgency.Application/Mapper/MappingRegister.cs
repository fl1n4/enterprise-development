using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Entities;
using Mapster;

namespace RealEstateAgency.Application.Mapper;
/// <summary>
/// Global Mapster configuration for mapping between Domain and DTO models.
/// </summary>
public class MappingRegister : IRegister
{
    /// <summary>
    /// Registers mappings between domain models and DTOs.
    /// </summary>
    /// <param name="config">The TypeAdapterConfig instance used to define mapping rules.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Client, ClientDto>();

        // === Real Estate Object ===
        config.NewConfig<RealEstateObject, RealEstateObjectDto>();

        // === Request ===
        config.NewConfig<Request, RequestDto>();
    }
}