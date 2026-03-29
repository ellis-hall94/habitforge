using HabitForge.Api.Extensions;
using HabitForge.Api.Middleware;
using HabitForge.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHabitForgeDatabase(builder.Configuration);
builder.Services.AddHabitForgeAuthentication(builder.Configuration);
builder.Services.AddHabitForgeSwagger();
builder.Services.AddHabitForgeServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCors", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    options.AddPolicy("ProductionCors", policy =>
        policy.WithOrigins("https://habitforge.example.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HabitForgeDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseCors(app.Environment.IsDevelopment() ? "DevelopmentCors" : "ProductionCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
