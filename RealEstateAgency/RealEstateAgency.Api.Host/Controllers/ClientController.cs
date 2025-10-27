using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Contracts.Client;

namespace RealEstateAgency.Api.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientCRUDService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> Get(int id)
    {
        try { return Ok(await service.Get(id)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAll() =>
        Ok(await service.GetAll());

    [HttpPost]
    public async Task<ActionResult<ClientDto>> Create(ClientCreateUpdateDto dto) =>
        Ok(await service.Create(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientDto>> Update(int id, ClientCreateUpdateDto dto) =>
        Ok(await service.Update(dto, id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}