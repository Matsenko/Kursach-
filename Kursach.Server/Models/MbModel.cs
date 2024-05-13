using Newtonsoft.Json;

namespace Kursach.Models;

public class MbModel
{

    public int CurrencyCodeA { get; set; }
    public int CurrencyCodeB { get; set; }
    public long Date { get; set; }
    public double RateSell { get; set; }
    public double RateBuy { get; set; }
    public double RateCross { get; set; }
}