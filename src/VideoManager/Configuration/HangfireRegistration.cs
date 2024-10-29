using Hangfire;
using Hangfire.MemoryStorage;
using VideoManager.Domains.FileProcessors;

namespace VideoManager.Configuration;

internal class HangfireRegistration
{
    internal static void RegisterHangfire(ServiceProvider serviceProvider, int minuteInterval)
    {
        // Configure Hangfire with MemoryStorage
        GlobalConfiguration.Configuration
            .UseMemoryStorage()
            .UseRecommendedSerializerSettings()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseColouredConsoleLogProvider()
            .UseActivator(new HangfireJobActivator(serviceProvider));

        // Schedule the Recurring Job
        RecurringJob.AddOrUpdate<IFileProcessor>("video-manager", fp => fp.ProcessVideos(), $"*/{minuteInterval} * * * *");
    }
}

internal class HangfireJobActivator(ServiceProvider serviceProvider) : JobActivator
{
    public override object ActivateJob(Type type)
    {
        return serviceProvider.GetRequiredService(type);
    }
}
