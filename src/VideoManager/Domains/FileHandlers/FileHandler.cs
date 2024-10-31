
using VideoManager.Helpers;

namespace VideoManager.Domains.FileHandlers;

public class FileHandler(
    string videosToPublishFolder,
    string publishedVideosFolder,
    string errorVideosFolder,
    ILogger<FileHandler> logger) : IFileHandler
{
    public IEnumerable<string> GetFiles() => Directory.GetFiles(videosToPublishFolder);

    public void HandleFailure(string filePath) => this.HandleFile(filePath, false);

    public void HandleSuccess(string filePath) => this.HandleFile(filePath, true);

    /// <summary>
    /// Handles the moving of the file based on the isSuccess
    /// If Success: Move the file to the published videos folder
    /// If Failed: Move the file to the error videos folder
    /// </summary>
    /// <param name="filePath">The current path of the file</param>
    /// <param name="isSuccess">Indicates if it is success</param>
    private void HandleFile(string filePath, bool isSuccess)
    {
        string currentFolder = Path.GetDirectoryName(filePath)!;
        string fileName = Path.GetFileName(filePath);

        string destinationFolder = isSuccess ? publishedVideosFolder : errorVideosFolder;
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }

        string destinationFilePath = Path.Combine(destinationFolder, fileName);

        File.Move(filePath, destinationFilePath);

        logger.LogInformation(
            "{CurrentDateTime} - File: {fileName} moved from {currentFolder} to {destinationFolder}.",
            LogMessages.GetCurrentDateTime(), fileName, currentFolder, destinationFolder);
    }
}
