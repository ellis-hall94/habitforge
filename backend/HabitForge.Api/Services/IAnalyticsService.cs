using HabitForge.Api.Models.DTOs;

namespace HabitForge.Api.Services;

public interface IAnalyticsService
{
    Task<StreakResponse> GetStreaksByHabitIdAsync(Guid habitId, Guid userId);
    Task<List<HabitStreakResponse>> GetAllStreaksByUserIdAsync(Guid userId);
    Task<HabitTrendResponse> GetCompletionTrendAsync(Guid habitId, int days, Guid userId);
    Task<AnalyticsSummaryResponse> GetSummaryAsync(Guid userId);
}