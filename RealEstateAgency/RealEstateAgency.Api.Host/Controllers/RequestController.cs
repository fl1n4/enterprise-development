using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Api.Host.Controllers;

/// <summary>
/// API controller for managing <see cref="Request"/> entities
/// Provides endpoints for creating, reading, updating, and deleting requests using <see cref="IRequestCRUDService"/>
/// </summary>
/// <param name="service">Service for performing CRUD operations and analytical queries for requests</param>
/// <param name="logger">Logger for recording informational and error events related to request operations</param>
[ApiController]
[Route("api/[controller]")]
public class RequestController(IRequestCRUDService service, ILogger<RequestController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific request by its identifier
    /// </summary>
    /// <param name="id">The identifier of the request</param>
    /// <returns>The corresponding <see cref="RequestDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> Get(int id)
    {
        try
        {
            return Ok(await service.Get(id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Request with ID {Id} not found", id);
            return NotFound($"Request with ID {id} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving request with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Retrieves all requests
    /// </summary>
    /// <returns>A list of <see cref="RequestDto"/> entities</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<RequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RequestDto>>> GetAll()
    {
        try
        {
            return Ok(await service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all requests");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve requests");
        }
    }

    /// <summary>
    /// Creates a new request
    /// </summary>
    /// <param name="dto">The data transfer object containing request creation details</param>
    /// <returns>The created <see cref="RequestDto"/> entity</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> Create(RequestCreateUpdateDto dto)
    {
        try
        {
            var created = await service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid data provided when creating a request");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating request");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create request");
        }
    }

    /// <summary>
    /// Updates an existing request
    /// </summary>
    /// <param name="id">The identifier of the request to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="RequestDto"/> entity</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RequestDto>> Update(int id, RequestCreateUpdateDto dto)
    {
        try
        {
            return Ok(await service.Update(dto, id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Request with ID {Id} not found for update", id);
            return NotFound($"Request with ID {id} not found");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid update data for request ID {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating request ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update request");
        }
    }

    /// <summary>
    /// Deletes a request by its identifier
    /// </summary>
    /// <param name="id">The identifier of the request to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.Delete(id);
            return deleted ? NoContent() : NotFound($"Request with ID {id} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting request with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete request");
        }
    }

    /// <summary>
    /// Retrieves the client associated with a specific request
    /// </summary>
    /// <param name="requestId">The request identifier</param>
    /// <returns><see cref="ClientDto"/> associated with the request</returns>
    [HttpGet("{requestId}/client")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> GetClientByRequestId(int requestId)
    {
        try
        {
            var client = await service.GetClientByRequestId(requestId);
            return Ok(client);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Client for request ID {Id} not found", requestId);
            return NotFound($"Client for request ID {requestId} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving client for request ID {Id}", requestId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve client");
        }
    }

    /// <summary>
    /// Retrieves the client associated with a specific request
    /// </summary>
    /// <param name="requestId">The request identifier</param>
    /// <returns><see cref="ClientDto"/> associated with the request</returns>
    [HttpGet("{requestId}/property")]
    [ProducesResponseType(typeof(RealEstateObjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RealEstateObjectDto>> GetPropertyByRequestId(int requestId)
    {
        try
        {
            var property = await service.GetPropertyByRequestId(requestId);
            return Ok(property);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Property for request ID {Id} not found", requestId);
            return NotFound($"Property for request ID {requestId} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving property for request ID {Id}", requestId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve property");
        }
    }

    /// <summary>
    /// Counts requests grouped by property type
    /// </summary>
    /// <returns>Dictionary with <see cref="PropertyType"/> as key and count of requests as value</returns>
    [HttpGet("count-by-property-type")]
    [ProducesResponseType(typeof(Dictionary<PropertyType, int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Dictionary<PropertyType, int>>> GetRequestCountByPropertyType()
    {
        try
        {
            var result = await service.GetRequestCountByPropertyType();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error counting requests by property type");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve statistics");
        }
    }
}