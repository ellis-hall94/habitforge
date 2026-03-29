namespace HabitForge.Api.Models.DTOs;
public class StreakResponse
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly? LastCompletedDate { get; set; }
}