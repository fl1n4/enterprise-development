using RabbitMQ.Client;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Generator.Services;
using System.Text.Json;

namespace RealEstateAgency.Generator.RabbitMq.Host;

/// <summary>
/// Provides functionality for publishing client, real estate object,
/// and request data batches to RabbitMQ using a direct exchange
/// </summary>
public class RealEstateAgencyRabbitMqProducer(
    IConfiguration configuration,
    IConnection rabbitMqConnection,
    ILogger<RealEstateAgencyRabbitMqProducer> logger
) : IProducerService
{
    private readonly string _queueName =
        configuration.GetSection("RabbitMq")["QueueName"]
        ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");

    private const string ExchangeName = "real-estate.exchange";

    /// <summary>
    /// Ensures that the required RabbitMQ exchange and queue exist
    /// and binds all relevant routing keys
    /// </summary>
    private void EnsureExchangeAndQueue(IModel channel)
    {
        channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true);
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

        channel.QueueBind(_queueName, ExchangeName, "client.info");
        channel.QueueBind(_queueName, ExchangeName, "real-estate.object");
        channel.QueueBind(_queueName, ExchangeName, "request.data");
    }

    /// <summary>
    /// Publishes a batch of clients to RabbitMQ using the <c>client.info</c> routing key
    /// </summary>
    /// <param name="batch">A collection of client DTOs to send</param>
    /// <returns>A completed <see cref="Task"/> once the operation is finished</returns>
    public Task SendClientsAsync(IList<ClientCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} clients to exchange '{exchange}'", batch.Count, ExchangeName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            EnsureExchangeAndQueue(channel);

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "client.info",
                basicProperties: null,
                body: payload
            );

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while sending clients batch.");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Publishes a batch of real estate objects to RabbitMQ
    /// using the <c>real-estate.object</c> routing key
    /// </summary>
    /// <param name="batch">A collection of real estate object DTOs to send</param>
    /// <returns>A completed <see cref="Task"/> once the operation is finished</returns>
    public Task SendRealEstateObjectsAsync(IList<RealEstateObjectCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} real estate objects to exchange '{exchange}'", batch.Count, ExchangeName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            EnsureExchangeAndQueue(channel);

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "real-estate.object",
                basicProperties: null,
                body: payload
            );

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while sending real estate objects batch.");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Publishes a batch of requests to RabbitMQ using the <c>request.data</c> routing key
    /// </summary>
    /// <param name="batch">A collection of request DTOs to send</param>
    /// <returns>A completed <see cref="Task"/> once the operation is finished</returns>
    public Task SendRequestsAsync(IList<RequestCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} requests to exchange '{exchange}'", batch.Count, ExchangeName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            EnsureExchangeAndQueue(channel);

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "request.data",
                basicProperties: null,
                body: payload
            );

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while sending requests batch.");
            return Task.CompletedTask;
        }
    }
}