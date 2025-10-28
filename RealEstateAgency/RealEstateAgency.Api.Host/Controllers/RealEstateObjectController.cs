using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.RealEstateObject;

namespace RealEstateAgency.Api.Host.Controllers;

/// <summary>
/// API controller for managing <see cref="RealEstateObject"/> entities
/// Provides endpoints for creating, reading, updating, and deleting real estate objects using <see cref="IRealEstateObjectCRUDService"/>
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RealEstateObjectController(IRealEstateObjectCRUDService service) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific real estate object by its identifier
    /// </summary>
    /// <param name="id">The identifier of the real estate object</param>
    /// <returns>The corresponding <see cref="RealEstateObjectDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RealEstateObjectDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    /// <summary>
    /// Retrieves all real estate objects
    /// </summary>
    /// <returns>A list of <see cref="RealEstateObjectDto"/> entities</returns>
    [HttpGet]
    public async Task<ActionResult<List<RealEstateObjectDto>>> GetAll() =>
        Ok(await service.GetAll());

    /// <summary>
    /// Creates a new real estate object
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="RealEstateObjectDto"/> entity</returns>
    [HttpPost]
    public async Task<ActionResult<RealEstateObjectDto>> Create(RealEstateObjectCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    /// <summary>
    /// Updates an existing real estate object
    /// </summary>
    /// <param name="id">The identifier of the real estate object to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="RealEstateObjectDto"/> entity</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<RealEstateObjectDto>> Update(int id, RealEstateObjectCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    /// <summary>
    /// Deletes a real estate object by its identifier
    /// </summary>
    /// <param name="id">The identifier of the real estate object to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}