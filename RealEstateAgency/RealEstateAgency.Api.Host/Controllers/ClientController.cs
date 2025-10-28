using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Client;

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
}