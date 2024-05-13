using Kursach.Models;
using Kursach.Service;
using Kursach.Service.IService;
using Kursach.Services.IService;
using Kursach.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kursach.Controller;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IReadService _readService;
    private readonly IUserService _userService;

    public RequestController(IReadService readService,IUserService userService)
    {
        _readService = readService;
        _userService = userService;
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
    [HttpPut]
    public async Task<IActionResult> RegisterUser(string id)
    {
        UserDTO userDTO = new UserDTO
        {
            UserId = id
        };
        var registeredUser = await _userService.RegisterUser(userDTO);
        if (registeredUser != null)
        {
            return Ok();
        }
        else
        {
            return BadRequest("Invalid user data");
        }
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var deletedUser = await _userService.DeleteUser(id);
        if (deletedUser != null)
        {
            return Ok();
        }
        else
        {
            return BadRequest("User not found");
        }
    }

}