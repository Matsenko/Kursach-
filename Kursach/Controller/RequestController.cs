using Kursach.Models;
using Kursach.Service;
using Kursach.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Kursach.Controller;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IReadMbService _readMbService;

    public RequestController(IReadMbService readMbService)
    {
        _readMbService = readMbService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MbModel>>> Get()
    {
        
        var currencyData = await _readMbService.GetMbAsync();
        if (currencyData == null)
        {
            return NotFound();
        }
        Console.WriteLine("Fetched data: " + string.Join(", ", currencyData));
        return Ok(currencyData);
    }
}