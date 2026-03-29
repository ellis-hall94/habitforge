namespace HabitForge.Api.Models.DTOs;

public class HabitStreakResponse
{
    public Guid HabitId { get; set; }
    public string HabitName { get; set;} = string.Empty;
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly? LastCompletedDate { get; set; }
}