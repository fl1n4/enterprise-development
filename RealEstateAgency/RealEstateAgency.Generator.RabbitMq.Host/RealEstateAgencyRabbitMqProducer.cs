using RabbitMQ.Client;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Generator.RabbitMq.Host.Services;
using System.Text.Json;

public class RealEstateAgencyRabbitMqProducer(
    IConfiguration configuration,
    IConnection rabbitMqConnection,
    ILogger<RealEstateAgencyRabbitMqProducer> logger
) : IProducerService, IDisposable
{
    private readonly string _queueName =
        configuration.GetSection("RabbitMq")["QueueName"]
        ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");

    private const string ExchangeName = "real-estate.exchange";

    // Один канал на весь producer
    private readonly IModel _channel = rabbitMqConnection.CreateModel();

    /// <summary>
    /// Ensures that the required RabbitMQ exchange and queue exist
    /// and binds all relevant routing keys
    /// </summary>
    private void EnsureExchangeAndQueue()
    {
        _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true);

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(_queueName, ExchangeName, "client.info");
        _channel.QueueBind(_queueName, ExchangeName, "real-estate.object");
        _channel.QueueBind(_queueName, ExchangeName, "request.data");
    }

    /// <summary>
    /// Publishes a batch of clients to RabbitMQ using "client.info" routing key
    /// </summary>
    public Task SendClientsAsync(IList<ClientCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch);
        _channel.BasicPublish(ExchangeName, "client.info", null, payload);

        logger.LogInformation("Sent {count} clients", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publishes a batch of real estate objects to RabbitMQ using "real-estate.object" routing key
    /// </summary>
    public Task SendRealEstateObjectsAsync(IList<RealEstateObjectCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch);
        _channel.BasicPublish(ExchangeName, "real-estate.object", null, payload);

        logger.LogInformation("Sent {count} real estate objects", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publishes a batch of requests to RabbitMQ using "request.data" routing key
    /// </summary>
    public Task SendRequestsAsync(IList<RequestCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch);
        _channel.BasicPublish(ExchangeName, "request.data", null, payload);

        logger.LogInformation("Sent {count} requests", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the RabbitMQ channel
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        logger.LogInformation("RabbitMQ Producer disposed");
    }
}
