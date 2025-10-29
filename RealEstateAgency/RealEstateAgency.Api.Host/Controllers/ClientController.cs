using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Client;
using RealEstateAgency.Domain.Enums;

namespace RealEstateAgency.Api.Host.Controllers;

/// <summary>
/// API controller for managing <see cref="Client"/> entities
/// Provides endpoints for creating, reading, updating, and deleting clients using <see cref="IClientCRUDService"/>
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientCRUDService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific client by its identifier
    /// </summary>
    /// <param name="id">The identifier of the client</param>
    /// <returns>The corresponding <see cref="ClientDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    /// <summary>
    /// Retrieves all clients
    /// </summary>
    /// <returns>A list of <see cref="ClientDto"/> entities</returns>
    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll() =>
        Ok(await service.GetAll());

    /// <summary>
    /// Creates a new client
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="ClientDto"/> entity</returns>
    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(ClientCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    /// <summary>
    /// Updates an existing client
    /// </summary>
    /// <param name="id">The identifier of the client to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="ClientDto"/> entity</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ClientDto>> Update(int id, ClientCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    /// <summary>
    /// Deletes a client by its identifier
    /// </summary>
    /// <param name="id">The identifier of the client to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves clients who made sell requests within the specified period
    /// </summary>
    /// <param name="from">Start date of the period</param>
    /// <param name="to">End date of the period</param>
    /// <returns>List of <see cref="ClientDto"/> matching the criteria</returns>
    [HttpGet("sellers")]
    public async Task<ActionResult<IList<ClientDto>>> GetSellersByPeriod(DateOnly from, DateOnly to)
    {
        var clients = await service.GetSellersByPeriod(from, to);
        return Ok(clients);
    }

    /// <summary>
    /// Retrieves top 5 clients grouped by request type (Buy/Sell)
    /// </summary>
    /// <returns>Dictionary with <see cref="RequestType"/> as key and list of <see cref="ClientDto"/> as value</returns>
    [HttpGet("top-clients")]
    public async Task<ActionResult<Dictionary<RequestType, List<ClientDto>>>> GetTop5ClientsByRequestType()
    {
        var result = await service.GetTop5ClientsByRequestType();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves clients who submitted requests with the minimum amount
    /// </summary>
    /// <returns>List of <see cref="ClientDto"/> with the lowest request amount</returns>
    [HttpGet("clients-with-min-amount")]
    public async Task<ActionResult<IList<ClientDto>>> GetClientsWithMinRequestAmount()
    {
        var result = await service.GetClientsWithMinRequestAmount();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves clients who submitted buy requests for a specific property type
    /// </summary>
    /// <param name="type">The property type to filter by</param>
    /// <returns>List of <see cref="ClientDto"/> sorted alphabetically by full name</returns>
    [HttpGet("clients-by-property-type/{type}")]
    public async Task<ActionResult<IList<ClientDto>>> GetClientsByPropertyType(PropertyType type)
    {
        var result = await service.GetClientsByPropertyType(type);
        return Ok(result);
    }
}