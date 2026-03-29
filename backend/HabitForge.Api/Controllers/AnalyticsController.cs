using System.Security.Claims;
using HabitForge.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitForge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("streaks")]
    public async Task<IActionResult> GetAllStreaks()
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var streaks = await _analyticsService.GetAllStreaksByUserIdAsync(userId.Value);
        return Ok(streaks);
    }

    [HttpGet("habits/{id:guid}/streaks")]
    public async Task<IActionResult> GetHabitStreaks(Guid id)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var streaks = await _analyticsService.GetStreaksByHabitIdAsync(id, userId.Value);
        return Ok(streaks);
    }

    [HttpGet("habits/{id:guid}/trends")]
    public async Task<IActionResult> GetHabitTrend(Guid id, [FromQuery] int days = 30)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var trend = await _analyticsService.GetCompletionTrendAsync(id, days, userId.Value);
        return Ok(trend);
    }

        [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var summary = await _analyticsService.GetSummaryAsync(userId.Value);
        return Ok(summary);
    }

    private Guid? GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}