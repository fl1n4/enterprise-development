using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace RealEstateAgency.Infrastructure.RabbitMq;

/// <summary>
/// Background service responsible for consuming messages from RabbitMQ,
/// deserializing them, and delegating processing to the appropriate
/// scoped CRUD services
/// </summary>
public class RealEstateAgencyRabbitMqConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RealEstateAgencyRabbitMqConsumer> _logger;
    private readonly string _queueName;

    /// <summary>
    /// Initializes a new instance of the <see cref="RealEstateAgencyRabbitMqConsumer"/> class.
    /// </summary>
    /// <param name="connection">The RabbitMQ connection used to create channels</param>
    /// <param name="scopeFactory">Factory for creating scoped service providers</param>
    /// <param name="configuration">Application configuration containing queue settings</param>
    /// <param name="logger">Logger for diagnostic output</param>
    public RealEstateAgencyRabbitMqConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<RealEstateAgencyRabbitMqConsumer> logger)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;

        _queueName = configuration.GetSection("RabbitMq")["QueueName"]
            ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");
    }

    /// <summary>
    /// Starts the RabbitMQ consumer and begins listening for messages
    /// </summary>
    /// <param name="stoppingToken">Token used to signal cancellation of the background task</param>
    /// <returns>A completed <see cref="Task"/> once the consumer is initialized</returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Establishing RabbitMQ channel to queue '{queue}'", _queueName);

        stoppingToken.ThrowIfCancellationRequested();
        var channel = _connection.CreateModel();
        channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _logger.LogInformation("Started listening to queue '{queue}'", _queueName);
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (_, ea) => await ReceiveMessageAsync(ea, stoppingToken);

        channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles a received RabbitMQ message by deserializing the payload
    /// and delegating processing based on the routing key
    /// </summary>
    /// <param name="args">The event arguments containing message metadata and payload</param>
    /// <param name="stoppingToken">Cancellation token</param>
    private async Task ReceiveMessageAsync(BasicDeliverEventArgs args, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Received message from queue '{queue}' with routing key '{routingKey}'", _queueName, args.RoutingKey);

        try
        {
            stoppingToken.ThrowIfCancellationRequested();

            var json = Encoding.UTF8.GetString(args.Body.ToArray());

            using var scope = _scopeFactory.CreateScope();

            switch (args.RoutingKey)
            {
                case "real-estate.object":
                    await ProcessRealEstateObject(json, scope);
                    break;
                case "client.info":
                    await ProcessClient(json, scope);
                    break;
                case "request.data":
                    await ProcessRequest(json, scope);
                    break;
                default:
                    _logger.LogWarning("Unknown routing key: {routingKey}", args.RoutingKey);
                    return;
            }

            _logger.LogInformation("Processed message from queue '{queue}' with routing key '{routingKey}'", _queueName, args.RoutingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing message from queue '{queue}'", _queueName);
        }
    }

    /// <summary>
    /// Processes a batch of real estate object DTOs and persists them using the scoped CRUD service
    /// </summary>
    /// <param name="json">The raw JSON payload from the message</param>
    /// <param name="scope">A scoped service provider instance</param>
    private async Task ProcessRealEstateObject(string json, IServiceScope scope)
    {
        var realEstateObjects = JsonSerializer.Deserialize<List<RealEstateObjectCreateUpdateDto>>(json)
            ?? throw new FormatException("Unable to deserialize RealEstateObjectCreateUpdateDto list from message body");

        var service = scope.ServiceProvider.GetRequiredService<IRealEstateObjectCRUDService>();
        foreach (var dto in realEstateObjects)
        {
            await service.Create(dto);
        }
    }

    /// <summary>
    /// Processes a batch of client DTOs and persists them using the scoped CRUD service
    /// </summary>
    /// <param name="json">The raw JSON payload from the message</param>
    /// <param name="scope">A scoped service provider instance</param>
    private async Task ProcessClient(string json, IServiceScope scope)
    {
        var clients = JsonSerializer.Deserialize<List<ClientCreateUpdateDto>>(json)
            ?? throw new FormatException("Unable to deserialize ClientCreateUpdateDto list from message body");

        var service = scope.ServiceProvider.GetRequiredService<IClientCRUDService>();
        foreach (var dto in clients)
        {
            await service.Create(dto);
        }
    }

    /// <summary>
    /// Processes a batch of request DTOs and persists them using the scoped CRUD service
    /// </summary>
    /// <param name="json">The raw JSON payload from the message</param>
    /// <param name="scope">A scoped service provider instance</param>
    private async Task ProcessRequest(string json, IServiceScope scope)
    {
        var requests = JsonSerializer.Deserialize<List<RequestCreateUpdateDto>>(json)
            ?? throw new FormatException("Unable to deserialize RequestCreateUpdateDto list from message body");

        var service = scope.ServiceProvider.GetRequiredService<IRequestCRUDService>();
        foreach (var dto in requests)
        {
            await service.Create(dto);
        }
    }
}