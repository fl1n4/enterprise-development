using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;

namespace RealEstateAgency.Generator.RabbitMq.Host.Services;

/// <summary>
/// Defines a producer service responsible for publishing generated data
/// to a message broker (RabbitMQ)
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Publishes a batch of client records
    /// </summary>
    /// <param name="batch">List of client DTOs to send</param>
    public Task SendClientsAsync(IList<ClientCreateUpdateDto> batch);

    /// <summary>
    /// Publishes a batch of real estate object records
    /// </summary>
    /// <param name="batch">List of real estate DTOs to send</param>
    public Task SendRealEstateObjectsAsync(IList<RealEstateObjectCreateUpdateDto> batch);

    /// <summary>
    /// Publishes a batch of request records
    /// </summary>
    /// <param name="batch">List of request DTOs to send</param>
    public Task SendRequestsAsync(IList<RequestCreateUpdateDto> batch);
}