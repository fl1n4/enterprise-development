using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.AircraftModel;
using RealEstateAgency.Application.Contracts.Flight;
using RealEstateAgency.Application.Contracts.Passenger;
using RealEstateAgency.Application.Contracts.Ticket;
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
        config.NewConfig<Ticket, TicketDto>();

        config.NewConfig<TicketCreateUpdateDto, Ticket>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Flight)
            .Ignore(dest => dest.Passenger);

        config.NewConfig<Passenger, PassengerDto>();

        config.NewConfig<PassengerCreateUpdateDto, Passenger>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Tickets!);

        config.NewConfig<Flight, FlightDto>();

        config.NewConfig<FlightCreateUpdateDto, Flight>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.AircraftModel)
            .Ignore(dest => dest.Tickets!);

        config.NewConfig<AircraftModel, AircraftModelDto>();

        config.NewConfig<AircraftFamily, AircraftFamilyDto>();
    }
}