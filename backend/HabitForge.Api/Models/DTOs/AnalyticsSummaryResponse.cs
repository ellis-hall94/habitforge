namespace HabitForge.Api.Models.DTOs;

public class AnalyticsSummaryResponse
{
    public int TotalHabits { get; set; }
    public int TotalCompletionsThisWeek { get; set; }
    public double OverallCompletionRate { get; set; }
    public List<HabitStreakResponse> HabitStreaks { get; set; } = new();
}