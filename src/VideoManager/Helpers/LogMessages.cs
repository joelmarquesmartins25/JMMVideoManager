namespace VideoManager.Helpers;

public static class LogMessages
{
    public const string FileProcessorStarted = "File Processor started";
    public const string NoFilesFound = "No files found";
    public const string StartedProcessingFile = "Started processing of the file";
    public const string FailedToGenerateMetadata = "Failed to generate metadata";
    public const string FailedToUploadVideo = "Failed to upload video";
    public const string SuccessfullyProcessedFile = "Successfully processed file";
    public const string FileProcessorFinished = "File Processor finished";

    public static string GetCurrentDateTime() => DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss");
}
