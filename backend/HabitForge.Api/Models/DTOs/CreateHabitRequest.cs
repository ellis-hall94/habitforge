using System.ComponentModel.DataAnnotations;

namespace HabitForge.Api.Models.DTOs;

public class CreateHabitRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }
}
