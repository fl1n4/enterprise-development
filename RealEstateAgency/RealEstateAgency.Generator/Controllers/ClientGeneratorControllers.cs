using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Generator.Generator;
using RealEstateAgency.Generator.Services;
using System.Collections.Generic;

namespace RealEstateAgency.Generator.Controllers;

/// <summary>
/// Контроллер для запуска генерации и отправки DTO объектов недвижимости через брокер сообщений
/// </summary>
/// <param name="logger"></param>
/// <param name="producerService"></param>
[Route("api/[controller]")]
[ApiController]
public class ClientGeneratorController(ILogger<ClientGeneratorController> logger, IProducerService producerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ClientCreateUpdateDto>>> Get([FromQuery] int batchSize, [FromQuery] int payloadLimit, [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} clients via {batchSize} batches and {waitTime}s delay", payloadLimit, batchSize, waitTime);
        try
        {
            var list = new List<ClientCreateUpdateDto>(payloadLimit);
            var counter = 0;
            while (counter < payloadLimit)
            {
                var batch = ClientGenerator.GenerateClients(batchSize);
                await producerService.SendClientsAsync(batch);
                logger.LogInformation("Batch of {batchSize} clients has been sent", batchSize);
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