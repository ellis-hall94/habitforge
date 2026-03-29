using HabitForge.Api.Data;
using HabitForge.Api.Models.DTOs;
using HabitForge.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HabitForge.Tests.Services;

public class HabitServiceTests
{
    private static HabitForgeDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<HabitForgeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            return new HabitForgeDbContext(options);
    }

    [Fact]
    public async Task CreateHabitAsync_ReturnsHabitWithCorrectName()
    {
        using var dbContext = CreateDbContext();
        var service = new HabitService(dbContext);
        var userId = Guid.NewGuid();
        var request = new CreateHabitRequest { Name = "Exercise", Description = "Daily workout" };

        var result = await service.CreateHabitAsync(request, userId);

        Assert.Equal("Exercise", result.Name);
        Assert.Equal("Daily workout", result.Description);
        Assert.Equal(userId, result.UserId);
        Assert.False(result.IsArchived);
    }

    [Fact]
    public async Task GetAllHabitsByUserIdAsync_ReturnsOnlyUserHabits()
    {
        using var dbContext = CreateDbContext();
        var service = new HabitService(dbContext);
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        await service.CreateHabitAsync(new CreateHabitRequest { Name = "Read" }, userId);
        await service.CreateHabitAsync(new CreateHabitRequest { Name = "Meditate" }, userId);
        await service.CreateHabitAsync(new CreateHabitRequest { Name = "Other User Habit" }, otherUserId);

        var results = (await service.GetAllHabitsByUserIdAsync(userId)).ToList();

        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.Equal(userId, r.UserId));
    }

    [Fact]
    public async Task GetHabitByIdAsync_WrongUser_ReturnsNull()
    {
        using var dbContext = CreateDbContext();
        var service = new HabitService(dbContext);
        var userId = Guid.NewGuid();

        var created = await service.CreateHabitAsync(new CreateHabitRequest { Name = "Read" }, userId);

        var result = await service.GetHabitByIdAsync(created.Id, Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteHabitAsync_ExistingHabit_ReturnsTrueAndRemoves()
    {
        using var dbContext = CreateDbContext();
        var service = new HabitService(dbContext);
        var userId = Guid.NewGuid();

        var created = await service.CreateHabitAsync(new CreateHabitRequest { Name = "Read" }, userId);

        var deleted = await service.DeleteHabitAsync(created.Id, userId);
        var afterDelete = await service.GetHabitByIdAsync(created.Id, userId);

        Assert.True(deleted);
        Assert.Null(afterDelete);
    }

    [Fact]
    public async Task DeleteHabitAsync_NonExistentHabit_ReturnsFalse()
    {
        using var dbContext = CreateDbContext();
        var service = new HabitService(dbContext);

        var result = await service.DeleteHabitAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.False(result);
    }
}