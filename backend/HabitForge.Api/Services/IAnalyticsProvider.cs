using HabitForge.Api.Models.DTOs;

namespace HabitForge.Api.Services;

public interface IAnalyticsProvider
{
    Task<AnalyticsSummaryResponse> GenerateInsightsAsync(Guid userId);
}