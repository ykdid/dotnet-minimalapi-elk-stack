namespace MinimalApiElk.Models;

public class LogData
{
    public string? Message { get; set; }
    public string? Level { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}