using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RealEstateAgency.Application.Contracts.Client;

namespace RealEstateAgency.Infrastructure.RabbitMq;
    /// <summary>
    /// Служба для чтения данных из очереди RabbitMQ (RealEstateAgency)
    /// </summary>
    public class RealEstateAgencyRabbitMqConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RealEstateAgencyRabbitMqConsumer> _logger;
        private readonly string _queueName;

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

        /// <inheritdoc/>
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
        /// Обработка полученного сообщения
        /// </summary>
        private async Task ReceiveMessageAsync(BasicDeliverEventArgs args, CancellationToken stoppingToken)
        {
            _logger.LogInformation("Received message from queue '{queue}'", _queueName);

            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var propertyContracts = JsonSerializer.Deserialize<List<ClientCreateUpdateDto>>(json)
                    ?? throw new FormatException("Unable to deserialize PropertyCreateUpdateDto list from message body");

                using var scope = _scopeFactory.CreateScope();
                var propertyService = scope.ServiceProvider.GetRequiredService<IPropertyService>();
                await propertyService.ReceiveContractList(propertyContracts);

                _logger.LogInformation("Processed {count} property contracts from queue '{queue}'", propertyContracts.Count, _queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing message from queue '{queue}'", _queueName);
            }
        }
    }