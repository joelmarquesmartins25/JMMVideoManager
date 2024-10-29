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
        var files = fileHandler.GetFiles();
        if (!files.Any())
        {
            logger.LogInformation(LogMessages.NoFilesFound);
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
            logger.LogInformation("Started processing of the file: \"{fileName}\"...", fileName);

            // Generate metadata
            VideoFile? videoFile = await metadataGenerator.GenerateMetadataAsync(fileName);
            if (videoFile == null)
            {
                logger.LogWarning(LogMessages.FailedToGenerateMetadata);
                fileHandler.HandleFailure(filePath);
                continue;
            }

            // Upload video
            bool isSuccess = await videoPublisher.UploadVideoAsync(videoFile);
            if (!isSuccess)
            {
                logger.LogWarning(LogMessages.FailedToUploadVideo);
                fileHandler.HandleFailure(filePath);
                continue;
            }

            logger.LogInformation(LogMessages.SuccessfullyProcessedFile);
            fileHandler.HandleSuccess(filePath);
        }
    }
}
