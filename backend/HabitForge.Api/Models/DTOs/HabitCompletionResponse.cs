namespace HabitForge.Api.Models.DTOs;

public class HabitCompletionResponse
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public DateOnly CompletedDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
