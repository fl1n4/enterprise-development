using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Application.Contracts.Request;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Api.Host.Controllers;

/// <summary>
/// API controller for managing <see cref="Client"/> entities
/// Provides endpoints for creating, reading, updating, and deleting clients using <see cref="IClientCRUDService"/>
/// </summary>
/// <param name="service">Service for performing CRUD operations and related analytics on client</param>
/// <param name="logger">Logger for recording operational and error information</param>
[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientCRUDService service, ILogger<ClientController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific client by its identifier
    /// </summary>
    /// <param name="id">The identifier of the client</param>
    /// <returns>The corresponding <see cref="ClientDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> Get(int id)
    {
        try
        {
            return Ok(await service.Get(id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Client with ID {Id} not found", id);
            return NotFound($"Client with ID {id} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving client with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>A list of <see cref="ClientDto"/> entities</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ClientDto>>> GetAll()
    {
        try
        {
            return Ok(await service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving all clients");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Creates a new client
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="ClientDto"/> entity</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> Create(ClientCreateUpdateDto dto)
    {
        try
        {
            if (dto == null)
                return BadRequest("Client data cannot be null");

            return Ok(await service.Create(dto));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating client");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing client
    /// </summary>
    /// <param name="id">The identifier of the client to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="ClientDto"/> entity</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDto>> Update(int id, ClientCreateUpdateDto dto)
    {
        try
        {
            return Ok(await service.Update(dto, id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Client with ID {Id} not found for update", id);
            return NotFound($"Client with ID {id} not found");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid data provided for client update (ID: {Id})", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating client with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a client by its identifier
    /// </summary>
    /// <param name="id">The identifier of the client to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting client with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves all requests of a specific client.
    /// </summary>
    /// <param name="clientId">The identifier of the client.</param>
    /// <returns>List of requests associated with the client.</returns>
    [HttpGet("{clientId}/requests")]
    [ProducesResponseType(typeof(IList<RequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RequestDto>>> GetClientRequests(int clientId)
    {
        try
        {
            var client = await service.Get(clientId);
            if (client == null)
                return NotFound($"Client with ID {clientId} not found");

            var requests = await service.GetRequestsByClientId(clientId);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving requests for client {Id}", clientId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves clients who made sell requests within the specified period
    /// </summary>
    /// <param name="from">Start date of the period</param>
    /// <param name="to">End date of the period</param>
    /// <returns>List of <see cref="ClientDto"/> matching the criteria</returns>
    [HttpGet("sellers")]
    [ProducesResponseType(typeof(IList<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<ClientDto>>> GetSellersByPeriod(DateOnly from, DateOnly to)
    {
        try
        {
            var clients = await service.GetSellersByPeriod(from, to);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving sellers between {From} and {To}", from, to);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves top 5 clients grouped by request type (Buy/Sell)
    /// </summary>
    /// <returns>Dictionary with <see cref="RequestType"/> as key and list of <see cref="ClientDto"/> as value</returns>
    [HttpGet("top-clients")]
    [ProducesResponseType(typeof(Dictionary<RequestType, List<ClientDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Dictionary<RequestType, List<ClientDto>>>> GetTop5ClientsByRequestType()
    {
        try
        {
            var result = await service.GetTop5ClientsByRequestType();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving top 5 clients by request type");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves clients who submitted requests with the minimum amount
    /// </summary>
    /// <returns>List of <see cref="ClientDto"/> with the lowest request amount</returns>
    [HttpGet("clients-with-min-amount")]
    [ProducesResponseType(typeof(IList<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<ClientDto>>> GetClientsWithMinRequestAmount()
    {
        try
        {
            var result = await service.GetClientsWithMinRequestAmount();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving clients with minimal request amount");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves clients who submitted buy requests for a specific property type
    /// </summary>
    /// <param name="type">The property type to filter by</param>
    /// <returns>List of <see cref="ClientDto"/> sorted alphabetically by full name</returns>
    [HttpGet("clients-by-property-type/{type}")]
    [ProducesResponseType(typeof(IList<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<ClientDto>>> GetClientsByPropertyType(PropertyType type)
    {
        try
        {
            var result = await service.GetClientsByPropertyType(type);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving clients by property type {Type}", type);
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}