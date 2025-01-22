using Common.DTOs;
using Domain.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace P2P.Api.Controllers;

[ApiController, Route("api/p2p")]
public class P2PController(IP2PService service) : Controller
{
    private readonly IP2PService _service = service;

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] GenerateDto dto)
    {
        try
        {
            await _service.Generate(dto).ConfigureAwait(false);

            return Accepted();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}