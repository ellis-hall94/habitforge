using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HabitForge.Api.Models.DTOs;
using HabitForge.Api.Configuration;
using HabitForge.Api.Data;
using HabitForge.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HabitForge.Api.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HabitForgeDbContext _dbContext;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(HabitForgeDbContext dbContext, IOptions<JwtSettings> jwtSettings)
    {
        _dbContext = dbContext;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser is not null) 
                throw new InvalidOperationException("A user with this email already exists.");

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    DisplayName = request.DisplayName,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                return GenerateAuthenticationResponse(user);
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid email or password.");

        return GenerateAuthenticationResponse(user);
    }

    private AuthenticationResponse GenerateAuthenticationResponse(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new AuthenticationResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}