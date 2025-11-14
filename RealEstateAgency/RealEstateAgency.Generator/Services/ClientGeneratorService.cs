using RealEstateAgency.Generator.Generator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RealEstateAgency.Generator.Services;

public class ClientGeneratorService(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<ClientGeneratorService> logger) : BackgroundService
{
    private readonly string _batchSize = configuration.GetSection("Generator:Client")["BatchSize"] ?? throw new KeyNotFoundException("BatchSize section of Generator:Client is missing");
    private readonly string _payloadLimit = configuration.GetSection("Generator:Client")["PayloadLimit"] ?? throw new KeyNotFoundException("PayloadLimit section of Generator:Client is missing");
    private readonly string _waitTime = configuration.GetSection("Generator:Client")["WaitTime"] ?? throw new KeyNotFoundException("WaitTime section of Generator:Client is missing");

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ClientGeneratorService started with {batch} batch size, {limit} payload limit, {wait}s wait time", _batchSize, _payloadLimit, _waitTime);

        if (!int.TryParse(_batchSize, out var batchSize)) throw new FormatException("Unable to parse BatchSize");
        if (!int.TryParse(_payloadLimit, out var payloadLimit)) throw new FormatException("Unable to parse PayloadLimit");
        if (!int.TryParse(_waitTime, out var waitTime)) throw new FormatException("Unable to parse WaitTime");

        var counter = 0;
        using var scope = scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();

        while (counter < payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var clients = ClientGenerator.GenerateClients(batchSize);
            await producer.SendClientsAsync(clients);

            await Task.Delay(waitTime * 1000, stoppingToken);
            counter += batchSize;
        }

        logger.LogInformation("ClientGeneratorService finished sending {total} messages", counter);
    }
}
