namespace HabitForge.Api.Models.Domain;

public class HabitCompletion
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public DateOnly CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Habit Habit { get; set; } = null!;
}
