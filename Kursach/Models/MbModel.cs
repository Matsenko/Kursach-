namespace Kursach.Models;

public class MbModel
{
    
    public long Id { get; set; }
    public DateTime PointDate { get; set; }
    public DateTime Date { get; set; }
    public float Ask { get; set; }
    public float Desk { get; set; }
    public float TrendAsk { get; set; }
    public float TrendBid { get; set; }
    public string Currency { get; set; }
    public string? Comment { get; set; }
}