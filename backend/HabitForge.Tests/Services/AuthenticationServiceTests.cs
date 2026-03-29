using HabitForge.Api.Configuration;
using HabitForge.Api.Data;
using HabitForge.Api.Models.DTOs;
using HabitForge.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;


namespace HabitForge.Tests.Services;

public class AuthenticationServiceTests
{
    private static HabitForgeDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<HabitForgeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new HabitForgeDbContext(options);
    }

    private static IOptions<JwtSettings> CreateJwtSettings()
    {
        return Options.Create(new JwtSettings
        {
            Secret = "ThisIsATestSecretKeyThatIsAtLeast32Characters",
            Issuer = "HabitForge",
            Audience = "HabitForgeClients",
            ExpiryMinutes = 60
        });
    }

    [Fact]
    public async Task RegisterAsync_ReturnsTokenForNewUser()
    {
        using var dbContext = CreateDbContext();
        var service = new AuthenticationService(dbContext, CreateJwtSettings());
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123",
            DisplayName = "Test User"
        };

        var result = await service.RegisterAsync(request);

        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ThrowsInvalidOperationException()
    {
        using var dbContext = CreateDbContext();
        var service = new AuthenticationService(dbContext, CreateJwtSettings());
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123",
            DisplayName = "Test User"
        };

        await service.RegisterAsync(request);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.RegisterAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        using var dbContext = CreateDbContext();
        var service = new AuthenticationService(dbContext, CreateJwtSettings());
        await service.RegisterAsync(new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123",
            DisplayName = "Test User"
        });

        var result = await service.LoginAsync(new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123"
        });

        Assert.False(string.IsNullOrEmpty(result.Token));
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsInvalidOperationException()
    {
        using var dbContext = CreateDbContext();
        var service = new AuthenticationService(dbContext, CreateJwtSettings());
        await service.RegisterAsync(new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123",
            DisplayName = "Test User"
        });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.LoginAsync(new LoginRequest
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            }));
    }
}