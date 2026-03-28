using System.Text;
using HabitForge.Api.Configuration;
using HabitForge.Api.Data;
using HabitForge.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace HabitForge.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHabitForgeServices(this IServiceCollection services)
    {
        services.AddScoped<IHabitService, HabitService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }

    public static IServiceCollection AddHabitForgeAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings configuration section is missing or invalid.");

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        return services;
    }

    public static IServiceCollection AddHabitForgeSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HabitForge API",
                Version = "v1",
                Description = "Phase 1 — Backend Skeleton"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter your JWT Bearer token: Bearer {token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });

        return services;
    }

    public static IServiceCollection AddHabitForgeDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HabitForgeDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
