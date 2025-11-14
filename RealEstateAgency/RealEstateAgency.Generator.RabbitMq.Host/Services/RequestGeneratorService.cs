using RealEstateAgency.Generator.RabbitMq.Host.Generator;

namespace RealEstateAgency.Generator.RabbitMq.Host.Services;

/// <summary>
/// Background service responsible for generating request data in batches
/// and delivering it to the message broker using <see cref="IProducerService"/>
/// </summary>
public class RequestGeneratorService(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<RequestGeneratorService> logger) : BackgroundService
{
    private readonly string _batchSize = configuration.GetSection("Generator:Request")["BatchSize"] ?? throw new KeyNotFoundException("BatchSize section of Generator:Request is missing");
    private readonly string _payloadLimit = configuration.GetSection("Generator:Request")["PayloadLimit"] ?? throw new KeyNotFoundException("PayloadLimit section of Generator:Request is missing");
    private readonly string _waitTime = configuration.GetSection("Generator:Request")["WaitTime"] ?? throw new KeyNotFoundException("WaitTime section of Generator:Request is missing");

    /// <summary>
    /// Runs the generation loop that produces and publishes request batches
    /// based on the configured batch size, payload limit, and delay interval
    /// </summary>
    /// <param name="stoppingToken">Token signaling the service to stop execution</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RequestGeneratorService started with {batch} batch size, {limit} payload limit, {wait}s wait time", _batchSize, _payloadLimit, _waitTime);

        if (!int.TryParse(_batchSize, out var batchSize)) throw new FormatException("Unable to parse BatchSize");
        if (!int.TryParse(_payloadLimit, out var payloadLimit)) throw new FormatException("Unable to parse PayloadLimit");
        if (!int.TryParse(_waitTime, out var waitTime)) throw new FormatException("Unable to parse WaitTime");

        var counter = 0;
        using var scope = scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();

        while (counter < payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var requests = RequestGenerator.GenerateRequests(batchSize);
            await producer.SendRequestsAsync(requests);

            await Task.Delay(waitTime * 1000, stoppingToken);
            counter += batchSize;
        }

        logger.LogInformation("RequestGeneratorService finished sending {total} messages", counter);
    }
}