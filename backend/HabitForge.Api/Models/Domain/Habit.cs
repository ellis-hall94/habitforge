namespace HabitForge.Api.Models.Domain;

public class Habit
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsArchived { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<HabitCompletion> Completions { get; set; } = new List<HabitCompletion>();
}
