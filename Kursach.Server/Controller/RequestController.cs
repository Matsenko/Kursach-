using Kursach.Models;
using Kursach.Service;
using Kursach.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Kursach.Controller;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IReadService _readService;

    public RequestController(IReadService readService)
    {
        _readService = readService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MbModel>>> Get()
    {
        
        var currencyData = await _readService.GetMbAsync();
        if (currencyData == null)
        {
            return NotFound();
        }
        Console.WriteLine("Fetched data: " + string.Join(", ", currencyData));
        return Ok(currencyData);
    }
}