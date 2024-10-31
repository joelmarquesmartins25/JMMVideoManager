using System.Diagnostics.CodeAnalysis;
using VideoManager.Domains.FileHandlers;
using VideoManager.Domains.MetadataGenerators;
using VideoManager.Domains.VideoPublishers;
using VideoManager.Helpers;

namespace VideoManager.Domains.FileProcessors;

public class FileProcessor(
    IFileHandler fileHandler,
    IMetadataGenerator metadataGenerator,
    IVideoPublisher videoPublisher,
    ILogger<FileProcessor> logger) : IFileProcessor
{
    public async Task ProcessVideos(CancellationToken cancellationToken)
    {
        this.LogMessage(LogLevel.Information, LogMessages.FileProcessorStarted);

        var files = fileHandler.GetFiles();
        if (!files.Any())
        {
            this.LogMessage(LogLevel.Information, LogMessages.NoFilesFound);
            this.LogMessage(LogLevel.Information, LogMessages.FileProcessorFinished);
            return;
        }

        foreach (string filePath in files)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellationToken);
                return;
            }

            // Get the file name
            string fileName = Path.GetFileName(filePath);
            this.LogMessage(LogLevel.Information, LogMessages.StartedProcessingFile, fileName);

            // Generate metadata
            VideoFile? videoFile = await metadataGenerator.GenerateMetadataAsync(fileName);
            if (videoFile == null)
            {
                this.LogMessage(LogLevel.Warning, LogMessages.FailedToGenerateMetadata);
                fileHandler.HandleFailure(filePath);
                continue;
            }

            // Upload video
            bool isSuccess = await videoPublisher.UploadVideoAsync(videoFile);
            if (!isSuccess)
            {
                this.LogMessage(LogLevel.Warning, LogMessages.FailedToUploadVideo);
                fileHandler.HandleFailure(filePath);
                continue;
            }

            this.LogMessage(LogLevel.Information, LogMessages.SuccessfullyProcessedFile, fileName);
            fileHandler.HandleSuccess(filePath);
        }

        // Finished processing
        this.LogMessage(LogLevel.Information, LogMessages.FileProcessorFinished);
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "Improves readability")]
    private void LogMessage(LogLevel level, string message, string? fileName = null)
    {
        string formattedMessage = fileName is null
            ? $"{LogMessages.GetCurrentDateTime()} - {message}"
            : $"{LogMessages.GetCurrentDateTime()} - {message}: {fileName}";

        logger.Log(level, formattedMessage);
    }
}
