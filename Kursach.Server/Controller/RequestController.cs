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
    [HttpGet("{currency_code}")]
    public async Task<ActionResult<IEnumerable<MbModel>>> Get(string currency_code)
    {

        var currencyData = await _readService.GetMbAsync();
        if (currencyData == null)
        {
            return NotFound();
        }
        currencyData = currencyData.Where(currency => currency.CurrencyCodeA.ToString() == currency_code).ToList();
        currencyData = currencyData.OrderBy(currency => currency.CurrencyCodeA).ToList();
        Console.WriteLine("Fetched data: " + string.Join(", ", currencyData));
        return Ok(currencyData);
    }
    [HttpGet("{currency_code}/{currency_code2}/{amount}")]
    public async Task<ActionResult<double>> ConvertCurrencies(string currency_code, string currency_code2, double amount)
    {
        var currencyData = await _readService.GetMbAsync();

        if (currencyData == null || !currencyData.Any())
        {
            return NotFound("Currency data not available");
        }

        var firstCurrencyData = currencyData.FirstOrDefault(currency => currency.CurrencyCodeA.ToString() == currency_code);
        if (firstCurrencyData == null)
        {
            return NotFound($"Currency '{currency_code}' not found");
        }

        var secondCurrencyData = currencyData.FirstOrDefault(currency => currency.CurrencyCodeA.ToString() == currency_code2);
        if (secondCurrencyData == null)
        {
            return NotFound($"Currency '{currency_code2}' not found");
        }

        if (firstCurrencyData.RateBuy == 0 || secondCurrencyData.RateBuy == 0)
        {
            return BadRequest("Currency rates are not available for conversion");
        }

        try
        {
            double convertedAmount = (amount / firstCurrencyData.RateBuy) * secondCurrencyData.RateBuy;
            return Ok(convertedAmount);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error during currency conversion: {ex.Message}");
        }
    }
}