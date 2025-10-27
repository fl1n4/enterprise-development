using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Entities;
using Mapster;

namespace RealEstateAgency.Application.Mapper;
/// <summary>
/// Global Mapster configuration for mapping between Domain and DTO models.
/// </summary>
public class MappingRegister : Mapster.IRegister
{
    /// <summary>
    /// Registers mappings between domain models and DTOs.
    /// </summary>
    /// <param name="config">The TypeAdapterConfig instance used to define mapping rules.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Client, ClientDto>();
        config.NewConfig<RealEstateObject, RealEstateObjectDto>();
        config.NewConfig<Request, RequestDto>();

        config.NewConfig<ClientCreateUpdateDto, Client>();
        config.NewConfig<RealEstateObjectCreateUpdateDto, RealEstateObject>();
        config.NewConfig<RequestCreateUpdateDto, Request>();
    }
}