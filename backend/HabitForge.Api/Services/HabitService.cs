using HabitForge.Api.Data;
using HabitForge.Api.Models.Domain;
using HabitForge.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HabitForge.Api.Services;

public class HabitService : IHabitService
{
    private readonly HabitForgeDbContext _dbContext;

    public HabitService(HabitForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<HabitResponse>> GetAllHabitsByUserIdAsync(Guid userId)
    {
        var habits = await _dbContext.Habits
            .Where(h => h.UserId == userId)
            .ToListAsync();

        return habits.Select(MapToResponse);
    }

    public async Task<HabitResponse?> GetHabitByIdAsync(Guid habitId, Guid userId)
    {
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        return habit is null ? null : MapToResponse(habit);
    }

    public async Task<HabitResponse> CreateHabitAsync(CreateHabitRequest request, Guid userId)
    {
        var newHabit = new Habit
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        _dbContext.Habits.Add(newHabit);
        await _dbContext.SaveChangesAsync();

        return MapToResponse(newHabit);
    }

    public async Task<HabitResponse?> UpdateHabitAsync(Guid habitId, UpdateHabitRequest request, Guid userId)
    {
        var existingHabit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (existingHabit is null)
            return null;

        existingHabit.Name = request.Name;
        existingHabit.Description = request.Description;
        existingHabit.IsArchived = request.IsArchived;
        existingHabit.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return MapToResponse(existingHabit);
    }

    public async Task<bool> DeleteHabitAsync(Guid habitId, Guid userId)
    {
        var existingHabit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (existingHabit is null)
            return false;

        _dbContext.Habits.Remove(existingHabit);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private static HabitResponse MapToResponse(Habit habit) => new()
    {
        Id = habit.Id,
        Name = habit.Name,
        Description = habit.Description,
        CreatedAt = habit.CreatedAt,
        UpdatedAt = habit.UpdatedAt,
        IsArchived = habit.IsArchived,
        UserId = habit.UserId
    };

    public async Task<HabitCompletionResponse?> ToggleCompletionAsync(Guid habitId, DateOnly date, Guid userId)
    {
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (habit is null)
            return null;

        var existing = await _dbContext.HabitCompletions
            .FirstOrDefaultAsync(c => c.HabitId == habitId && c.CompletedDate == date);

        if (existing is not null)
        {
            _dbContext.HabitCompletions.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return null;
        }

        var completion = new HabitCompletion
        {
            Id = Guid.NewGuid(),
            HabitId = habitId,
            CompletedDate = date,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.HabitCompletions.Add(completion);
        await _dbContext.SaveChangesAsync();

        return MapToCompletionResponse(completion);
    }

    public async Task<IEnumerable<HabitCompletionResponse>> GetCompletionsAsync(Guid habitId, DateOnly from, DateOnly to, Guid userId)
    {
        var habit = await _dbContext.Habits
            .FirstOrDefaultAsync(h => h.Id == habitId && h.UserId == userId);

        if (habit is null)
            return Enumerable.Empty<HabitCompletionResponse>();

        var completions = await _dbContext.HabitCompletions
            .Where(c => c.HabitId == habitId && c.CompletedDate >= from && c.CompletedDate <= to)
            .OrderByDescending(c => c.CompletedDate)
            .ToListAsync();

        return completions.Select(MapToCompletionResponse);
    }

    private static HabitCompletionResponse MapToCompletionResponse(HabitCompletion completion) => new()
    {
        Id = completion.Id,
        HabitId = completion.HabitId,
        CompletedDate = completion.CompletedDate,
        CreatedAt = completion.CreatedAt
    };
}
