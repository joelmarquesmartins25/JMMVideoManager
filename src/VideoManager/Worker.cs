using VideoManager.Domains;

namespace VideoManager;

public class Worker(FileProcessor fileProcessor, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await fileProcessor.ProcessVideos();
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
