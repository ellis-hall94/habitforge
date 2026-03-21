using HabitForge.Api.Models.DTOs;

namespace HabitForge.Api.Services;

public interface IHabitService
{
    Task<IEnumerable<HabitResponse>> GetAllHabitsByUserIdAsync(Guid userId);
    Task<HabitResponse?> GetHabitByIdAsync(Guid habitId, Guid userId);
    Task<HabitResponse> CreateHabitAsync(CreateHabitRequest request, Guid userId);
    Task<HabitResponse?> UpdateHabitAsync(Guid habitId, UpdateHabitRequest request, Guid userId);
    Task<bool> DeleteHabitAsync(Guid habitId, Guid userId);
}
