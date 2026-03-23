using HabitForge.Api.Models.DTOs;

namespace HabitForge.Api.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> RegisterAsync (RegisterRequest request);
    Task<AuthenticationResponse> LoginAsync (LoginRequest request);
}