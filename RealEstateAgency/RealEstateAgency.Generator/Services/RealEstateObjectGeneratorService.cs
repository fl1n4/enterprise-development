using RealEstateAgency.Generator.Generator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RealEstateAgency.Generator.Services;
/// <summary>
/// Служба для генерации и отправки DTO объектов недвижимости через заданные интервалы
/// </summary>
/// <param name="configuration">Конфигурация</param>
/// <param name="scopeFactory">Фабрика контекста</param>
/// <param name="logger">Логгер</param>
public class RealEstateObjectGeneratorService(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<RealEstateObjectGeneratorService> logger) : BackgroundService
{
    private readonly string _batchSize = configuration.GetSection("Generator:RealEstateObject")["BatchSize"] ?? throw new KeyNotFoundException("BatchSize section of Generator:RealEstateObject is missing");
    private readonly string _payloadLimit = configuration.GetSection("Generator:RealEstateObject")["PayloadLimit"] ?? throw new KeyNotFoundException("PayloadLimit section of Generator:RealEstateObject is missing");
    private readonly string _waitTime = configuration.GetSection("Generator:RealEstateObject")["WaitTime"] ?? throw new KeyNotFoundException("WaitTime section of Generator:RealEstateObject is missing");

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RealEstateObjectGeneratorService started with {batch} batch size, {limit} payload limit, {wait}s wait time", _batchSize, _payloadLimit, _waitTime);

        if (!int.TryParse(_batchSize, out var batchSize)) throw new FormatException("Unable to parse BatchSize");
        if (!int.TryParse(_payloadLimit, out var payloadLimit)) throw new FormatException("Unable to parse PayloadLimit");
        if (!int.TryParse(_waitTime, out var waitTime)) throw new FormatException("Unable to parse WaitTime");

        var counter = 0;
        using var scope = scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();

        while (counter < payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var realEstates = RealEstateObjectGenerator.GenerateRealEstateObjects(batchSize);
            await producer.SendRealEstateObjectsAsync(realEstates);

            await Task.Delay(waitTime * 1000, stoppingToken);
            counter += batchSize;
        }

        logger.LogInformation("RealEstateObjectGeneratorService finished sending {total} messages", counter);
    }
}
