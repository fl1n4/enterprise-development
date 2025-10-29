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
[ApiController]
[Route("api/[controller]")]
public class RequestController(IRequestCRUDService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific request by its identifier
    /// </summary>
    /// <param name="id">The identifier of the request</param>
    /// <returns>The corresponding <see cref="RequestDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    /// <summary>
    /// Retrieves all requests
    /// </summary>
    /// <returns>A list of <see cref="RequestDto"/> entities</returns>
    [HttpGet]
    public async Task<ActionResult<List<RequestDto>>> GetAll() =>
        Ok(await service.GetAll());

    /// <summary>
    /// Creates a new request
    /// </summary>
    /// <param name="dto">The data transfer object containing request creation details</param>
    /// <returns>The created <see cref="RequestDto"/> entity</returns>
    [HttpPost]
    public async Task<ActionResult<RequestDto>> Create(RequestCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    /// <summary>
    /// Updates an existing request
    /// </summary>
    /// <param name="id">The identifier of the request to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="RequestDto"/> entity</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<RequestDto>> Update(int id, RequestCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    /// <summary>
    /// Deletes a request by its identifier
    /// </summary>
    /// <param name="id">The identifier of the request to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves the client associated with a specific request
    /// </summary>
    /// <param name="requestId">The request identifier</param>
    /// <returns><see cref="ClientDto"/> associated with the request</returns>
    [HttpGet("{requestId}/client")]
    public async Task<ActionResult<ClientDto>> GetClientByRequestId(int requestId)
    {
        var client = await service.GetClientByRequestId(requestId);
        return Ok(client);
    }

    /// <summary>
    /// Retrieves the client associated with a specific request
    /// </summary>
    /// <param name="requestId">The request identifier</param>
    /// <returns><see cref="ClientDto"/> associated with the request</returns>
    [HttpGet("{requestId}/property")]
    public async Task<ActionResult<RealEstateObjectDto>> GetPropertyByRequestId(int requestId)
    {
        var property = await service.GetPropertyByRequestId(requestId);
        return Ok(property);
    }

    /// <summary>
    /// Counts requests grouped by property type
    /// </summary>
    /// <returns>Dictionary with <see cref="PropertyType"/> as key and count of requests as value</returns>
    [HttpGet("count-by-property-type")]
    public async Task<ActionResult<Dictionary<PropertyType, int>>> GetRequestCountByPropertyType()
    {
        var result = await service.GetRequestCountByPropertyType();
        return Ok(result);
    }
}