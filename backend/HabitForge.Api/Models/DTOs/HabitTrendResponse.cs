namespace HabitForge.Api.Models.DTOs;

public class HabitTrendResponse
{
    public Guid HabitId { get; set; }
    public string HabitName { get; set;} = string.Empty;
    public List<CompletionTrendPoint> DataPoints { get; set; } = new();
}