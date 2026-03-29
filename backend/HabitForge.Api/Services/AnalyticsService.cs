using HabitForge.Api.Data;
using HabitForge.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HabitForge.Api.Services;

public class AnalyticsService : IAnalyticsService, IAnalyticsProvider
{
        private readonly HabitForgeDbContext _dbContext;

    public AnalyticsService(HabitForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StreakResponse> GetStreaksByHabitIdAsync(Guid habitId, Guid userId)
    {
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (habit is null)
            return new StreakResponse();

        var completedDates = await _dbContext.HabitCompletions
            .Where(c => c.HabitId == habitId)
            .OrderByDescending(c => c.CompletedDate)
            .Select(c => c.CompletedDate)
            .ToListAsync();

        return CalculateStreaks(completedDates);
    }

    public async Task<List<HabitStreakResponse>> GetAllStreaksByUserIdAsync(Guid userId)
    {
        var habits = await _dbContext.Habits
            .Where(h => h.UserId == userId && !h.IsArchived)
            .ToListAsync();

        var results = new List<HabitStreakResponse>();

        foreach (var habit in habits)
        {
            var completedDates = await _dbContext.HabitCompletions
                .Where(c => c.HabitId == habit.Id)
                .OrderByDescending(c => c.CompletedDate)
                .Select(c => c.CompletedDate)
                .ToListAsync();

            var streaks = CalculateStreaks(completedDates);

            results.Add(new HabitStreakResponse
            {
                HabitId = habit.Id,
                HabitName = habit.Name,
                CurrentStreak = streaks.CurrentStreak,
                LongestStreak = streaks.LongestStreak,
                LastCompletedDate = streaks.LastCompletedDate
            });
        }

        return results;
    }

        public async Task<HabitTrendResponse> GetCompletionTrendAsync(Guid habitId, int days, Guid userId)
    {
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (habit is null)
            return new HabitTrendResponse { HabitId = habitId };

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startDate = today.AddDays(-(days - 1));

        var completedDates = await _dbContext.HabitCompletions
            .Where(c => c.HabitId == habitId && c.CompletedDate >= startDate && c.CompletedDate <= today)
            .Select(c => c.CompletedDate)
            .ToListAsync();

        var completedDateSet = completedDates.ToHashSet();

        var dataPoints = new List<CompletionTrendPoint>();
        for (var date = startDate; date <= today; date = date.AddDays(1))
        {
            dataPoints.Add(new CompletionTrendPoint
            {
                Date = date,
                Completed = completedDateSet.Contains(date)
            });
        }

        return new HabitTrendResponse
        {
            HabitId = habit.Id,
            HabitName = habit.Name,
            DataPoints = dataPoints
        };
    }

    public async Task<AnalyticsSummaryResponse> GetSummaryAsync(Guid userId)
    {
        var habits = await _dbContext.Habits
            .Where(h => h.UserId == userId && !h.IsArchived)
            .ToListAsync();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var weekStart = today.AddDays(-(int)today.DayOfWeek);

        var weeklyCompletions = await _dbContext.HabitCompletions
            .Where(c => c.Habit.UserId == userId && c.CompletedDate >= weekStart && c.CompletedDate <= today)
            .CountAsync();

        var daysSinceWeekStart = today.DayNumber - weekStart.DayNumber + 1;
        var possibleCompletions = habits.Count * daysSinceWeekStart;
        var completionRate = possibleCompletions > 0
            ? (double)weeklyCompletions / possibleCompletions * 100
            : 0;

        var habitStreaks = await GetAllStreaksByUserIdAsync(userId);

        return new AnalyticsSummaryResponse
        {
            TotalHabits = habits.Count,
            TotalCompletionsThisWeek = weeklyCompletions,
            OverallCompletionRate = Math.Round(completionRate, 1),
            HabitStreaks = habitStreaks
        };
    }

    public Task<AnalyticsSummaryResponse> GenerateInsightsAsync(Guid userId)
    {
        return GetSummaryAsync(userId);
    }

        private static StreakResponse CalculateStreaks(List<DateOnly> completedDatesDescending)
    {
        if (completedDatesDescending.Count == 0)
            return new StreakResponse();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var lastCompleted = completedDatesDescending[0];

        if (lastCompleted != today && lastCompleted != today.AddDays(-1))
        {
            return new StreakResponse
            {
                CurrentStreak = 0,
                LongestStreak = CalculateLongestStreak(completedDatesDescending),
                LastCompletedDate = lastCompleted
            };
        }

        var currentStreak = 0;
        var expectedDate = lastCompleted;

        foreach (var date in completedDatesDescending)
        {
            if (date == expectedDate)
            {
                currentStreak++;
                expectedDate = expectedDate.AddDays(-1);
            }
            else if (date < expectedDate)
            {
                break;
            }
        }

        return new StreakResponse
        {
            CurrentStreak = currentStreak,
            LongestStreak = CalculateLongestStreak(completedDatesDescending),
            LastCompletedDate = lastCompleted
        };
    }

    private static int CalculateLongestStreak(List<DateOnly> completedDatesDescending)
    {
        if (completedDatesDescending.Count == 0)
            return 0;

        var sorted = completedDatesDescending.OrderBy(d => d).ToList();
        var longest = 1;
        var current = 1;

        for (var i = 1; i < sorted.Count; i++)
        {
            if (sorted[i].DayNumber - sorted[i - 1].DayNumber == 1)
            {
                current++;
                if (current > longest)
                    longest = current;
            }
            else
            {
                current = 1;
            }
        }

        return longest;
    }
}