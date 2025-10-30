using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.RealEstateObject;
using RealEstateAgency.Application.Contracts.Request;

namespace RealEstateAgency.Api.Host.Controllers;

/// <summary>
/// API controller for managing <see cref="RealEstateObject"/> entities
/// Provides endpoints for creating, reading, updating, and deleting real estate objects using <see cref="IRealEstateObjectCRUDService"/>
/// </summary>
/// <param name="service">Service for performing CRUD operations and related analytics on real estate objects</param>
/// <param name="logger">Logger for recording operational and error information</param>
[ApiController]
[Route("api/[controller]")]
public class RealEstateObjectController(IRealEstateObjectCRUDService service, ILogger<RealEstateObjectController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific real estate object by its identifier
    /// </summary>
    /// <param name="id">The identifier of the real estate object</param>
    /// <returns>The corresponding <see cref="RealEstateObjectDto"/> entity if found, otherwise NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RealEstateObjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RealEstateObjectDto>> Get(int id)
    {
        try
        {
            return Ok(await service.Get(id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Real estate object with ID {Id} not found", id);
            return NotFound($"Object with ID {id} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while retrieving real estate object with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    /// <summary>
    /// Retrieves all real estate objects
    /// </summary>
    /// <returns>A list of <see cref="RealEstateObjectDto"/> entities</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<RealEstateObjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RealEstateObjectDto>>> GetAll()
    {
        try
        {
            return Ok(await service.GetAll());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all real estate objects");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve data");
        }
    }

    /// <summary>
    /// Creates a new real estate object
    /// </summary>
    /// <param name="dto">The data transfer object containing creation details</param>
    /// <returns>The created <see cref="RealEstateObjectDto"/> entity</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RealEstateObjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RealEstateObjectDto>> Create(RealEstateObjectCreateUpdateDto dto)
    {
        try
        {
            var created = await service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid data provided for creating a real estate object");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating a real estate object");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create object");
        }
    }

    /// <summary>
    /// Updates an existing real estate object
    /// </summary>
    /// <param name="id">The identifier of the real estate object to update</param>
    /// <param name="dto">The data transfer object containing updated information</param>
    /// <returns>The updated <see cref="RealEstateObjectDto"/> entity</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RealEstateObjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RealEstateObjectDto>> Update(int id, RealEstateObjectCreateUpdateDto dto)
    {
        try
        {
            return Ok(await service.Update(dto, id));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Real estate object with ID {Id} not found for update", id);
            return NotFound($"Object with ID {id} not found");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid update data provided for object with ID {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating real estate object with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update object");
        }
    }

    /// <summary>
    /// Deletes a real estate object by its identifier
    /// </summary>
    /// <param name="id">The identifier of the real estate object to delete</param>
    /// <returns>NoContent if deletion was successful, otherwise NotFound</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await service.Delete(id);
            return deleted ? NoContent() : NotFound($"Object with ID {id} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting real estate object with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete object");
        }
    }

    /// <summary>
    /// Retrieves all requests related to a specific real estate object.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the real estate object.</param>
    /// <returns>
    /// 200 OK — if requests are found.<br/>
    /// 404 NotFound — if no requests exist for the specified property.<br/>
    /// 500 InternalServerError — in case of unexpected errors.
    /// </returns>
    [HttpGet("{propertyId}/requests")]
    [ProducesResponseType(typeof(List<RequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RequestDto>>> GetRequestsByPropertyId(int propertyId)
    {
        try
        {
            var result = await service.GetRequestsByPropertyId(propertyId);
            if (result == null || result.Count == 0)
                return NotFound($"No requests found for property with ID {propertyId}");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving requests for property ID {PropertyId}", propertyId);
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve requests");
        }
    }
}