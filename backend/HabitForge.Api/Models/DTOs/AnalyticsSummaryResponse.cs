namespace HabitForge.Api.Models.DTOs;

public class AnalyticsSummaryResponse
{
    public int TotalHabits { get; set; }
    public int totalCompletionsThisWeek { get; set; }
    public double OverallCompletionRate { get; set; }
    public List<HabitStreakResponse> Habitstreaks { get; set; } = new();
}