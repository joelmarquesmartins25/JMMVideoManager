using VideoManager;
using VideoManager.Domains;
using VideoManager.Domains.FileHandlers;
using VideoManager.Domains.MetadataGenerators;
using VideoManager.Domains.VideoPublishers;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddLogging();

// Get configurations
string videosToPublishFolder = configuration.GetValue<string>("VideosToPublishFolder")!;
string publishedVideosFolder = configuration.GetValue<string>("PublishedVideosFolder")!;
string errorVideosFolder = configuration.GetValue<string>("ErrorVideosFolder")!;
int timeintervalMinutes = configuration.GetValue<int>("TimeIntervalMinutes")!;

// Register domain services
builder.Services.AddScoped<IFileHandler>(sp => new FileHandler(
    videosToPublishFolder, publishedVideosFolder, errorVideosFolder, sp.GetRequiredService<ILogger<FileHandler>>()));
builder.Services.AddScoped<IMetadataGenerator, MetadataGenerator>();
builder.Services.AddScoped<IVideoPublisher, YouTubeVideoPublisher>();
builder.Services.AddScoped<FileProcessor>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
