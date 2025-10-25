using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.Application.Contracts;
internal class MappingRegister
{
}
﻿using RealEstateAgency.Application.Contracts.Client;
using AirCompany.Application.Contracts.AircraftModel;
using AirCompany.Application.Contracts.Flight;
using AirCompany.Application.Contracts.Passenger;
using AirCompany.Application.Contracts.Ticket;
using AirCompany.Domain.Model;
using Mapster;

namespace AirCompany.Application.Mapper;

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