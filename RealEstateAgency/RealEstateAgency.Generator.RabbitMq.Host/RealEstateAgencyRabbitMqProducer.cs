using RabbitMQ.Client;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Generator.Services;
using System.Text.Json;

namespace RealEstateAgency.Generator.RabbitMq.Host;

/// <summary>
/// Имплементация для отправки DTO через очередь RabbitMQ
/// </summary>
/// <param name="configuration">Конфигурация</param>
/// <param name="rabbitMqConnection">Подключение к брокеру сообщений</param>
/// <param name="logger">Логгер</param>
public class RealEstateAgencyRabbitMqProducer(
    IConfiguration configuration,
    IConnection rabbitMqConnection,
    ILogger<RealEstateAgencyRabbitMqProducer> logger) : IProducerService
{
    private readonly string _queueName = configuration.GetSection("RabbitMq")["QueueName"] ?? throw new KeyNotFoundException("QueueName section of RabbitMq is missing");

    /// <inheritdoc/>
    public Task SendClientsAsync(IList<ClientCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} clients to {queue}", batch.Count, _queueName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
            channel.BasicPublish(exchange: string.Empty, routingKey: "client.info", mandatory: false, body: payload);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} clients to {queue}", batch.Count, _queueName);
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc/>
    public Task SendRealEstateObjectsAsync(IList<RealEstateObjectCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} real estate objects to {queue}", batch.Count, _queueName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
            channel.BasicPublish(exchange: string.Empty, routingKey: "real-estate.object", mandatory: false, body: payload);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} real estate objects to {queue}", batch.Count, _queueName);
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc/>
    public Task SendRequestsAsync(IList<RequestCreateUpdateDto> batch)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} requests to {queue}", batch.Count, _queueName);
            var payload = JsonSerializer.SerializeToUtf8Bytes(batch);

            using var channel = rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
            channel.BasicPublish(exchange: string.Empty, routingKey: "request.data", mandatory: false, body: payload);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} requests to {queue}", batch.Count, _queueName);
            return Task.CompletedTask;
        }
    }
}