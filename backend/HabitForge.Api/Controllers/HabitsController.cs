using System.Security.Claims;
using HabitForge.Api.Models.DTOs;
using HabitForge.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitForge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitsController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllHabits()
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var habits = await _habitService.GetAllHabitsByUserIdAsync(userId.Value);
        return Ok(habits);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetHabitById(Guid id)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var habit = await _habitService.GetHabitByIdAsync(id, userId.Value);
        return habit is null ? NotFound() : Ok(habit);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit([FromBody] CreateHabitRequest request)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var createdHabit = await _habitService.CreateHabitAsync(request, userId.Value);
        return CreatedAtAction(nameof(GetHabitById), new { id = createdHabit.Id }, createdHabit);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateHabit(Guid id, [FromBody] UpdateHabitRequest request)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var updatedHabit = await _habitService.UpdateHabitAsync(id, request, userId.Value);
        return updatedHabit is null ? NotFound() : Ok(updatedHabit);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHabit(Guid id)
    {
        var userId = GetUserIdFromClaims();
        if (userId is null)
            return Unauthorized();

        var deleted = await _habitService.DeleteHabitAsync(id, userId.Value);
        return deleted ? NoContent() : NotFound();
    }

    private Guid? GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
