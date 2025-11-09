using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Generator.Generator;
using RealEstateAgency.Generator.Services;

public class RequestGeneratorController(ILogger<RequestGeneratorController> logger, IProducerService producerService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RequestCreateUpdateDto>>> Get([FromQuery] int batchSize, [FromQuery] int payloadLimit, [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} requests via {batchSize} batches and {waitTime}s delay", payloadLimit, batchSize, waitTime);
        try
        {
            var list = new List<RequestCreateUpdateDto>(payloadLimit);
            var counter = 0;
            while (counter < payloadLimit)
            {
                var batch = RequestGenerator.GenerateRequests(batchSize);
                await producerService.SendRequestsAsync(batch);
                logger.LogInformation("Batch of {batchSize} requests has been sent", batchSize);
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