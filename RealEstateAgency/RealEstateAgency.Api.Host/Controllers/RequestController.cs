using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Request;

namespace RealEstateAgency.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController(IRequestCRUDService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet]
    public async Task<ActionResult<List<RequestDto>>> GetAll() =>
        Ok(await service.GetAll());

    [HttpPost]
    public async Task<ActionResult<RequestDto>> Create(RequestCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult<RequestDto>> Update(int id, RequestCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}