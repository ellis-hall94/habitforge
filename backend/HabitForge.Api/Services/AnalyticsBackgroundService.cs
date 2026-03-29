namespace HabitForge.Api.Services;

public class AnalyticsBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AnalyticsBackgroundService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public AnalyticsBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<AnalyticsBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AnalyticsBackgroundService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "AnalyticsBackgroundService running at {time}",
                DateTime.UtcNow);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var provider = scope.ServiceProvider
                    .GetRequiredService<IAnalyticsProvider>();
                _logger.LogInformation(
                    "Analytics provider ready");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error in AnalyticsBackgroundService");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}