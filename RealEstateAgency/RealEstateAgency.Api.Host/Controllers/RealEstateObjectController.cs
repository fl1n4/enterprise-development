using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.RealEstateObject;

namespace RealEstateAgency.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RealEstateObjectController(IRealEstateObjectCRUDService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<RealEstateObjectDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet]
    public async Task<ActionResult<List<RealEstateObjectDto>>> GetAll() =>
        Ok(await service.GetAll());

    [HttpPost]
    public async Task<ActionResult<RealEstateObjectDto>> Create(RealEstateObjectCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult<RealEstateObjectDto>> Update(int id, RealEstateObjectCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}