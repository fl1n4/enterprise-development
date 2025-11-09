using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Generator.Generator;
using RealEstateAgency.Generator.Services;

namespace RealEstateAgency.Generator.Controllers;

/// <summary>
/// Контроллер для запуска генерации и отправки DTO объектов недвижимости через брокер сообщений
/// </summary>
/// <param name="logger"></param>
/// <param name="producerService"></param>
[Route("api/[controller]")]
[ApiController]
public class RealEstateObjectGeneratorController(ILogger<RealEstateObjectGeneratorController> logger, IProducerService producerService) : ControllerBase
{
    /// <summary>
    /// Метод для отправки DTO объектов недвижимости через брокер
    /// </summary>
    /// <param name="batchSize">Размер батча отправляемых сообщений</param>
    /// <param name="payloadLimit">Количество отправляемых сообщений</param>
    /// <param name="waitTime">Пауза в секундах между отправками батчей</param>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<RealEstateObjectCreateUpdateDto>>> Get([FromQuery] int batchSize, [FromQuery] int payloadLimit, [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} real estate objects via {batchSize} batches and {waitTime}s delay", payloadLimit, batchSize, waitTime);
        try
        {
            var list = new List<RealEstateObjectCreateUpdateDto>(payloadLimit);
            var counter = 0;
            while (counter < payloadLimit)
            {
                var batch = RealEstateObjectGenerator.GenerateRealEstateObjects(batchSize);
                await producerService.SendRealEstateObjectsAsync(batch);
                logger.LogInformation("Batch of {batchSize} real estate objects has been sent", batchSize);
                await Task.Delay(waitTime * 1000);
                counter += batchSize;
                list.AddRange(batch);
            }

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Get), GetType().Name);
            return Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(Get), GetType().Name);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}