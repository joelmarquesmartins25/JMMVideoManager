namespace VideoUploader.Domains;

public class FileProcessor
{
    private readonly IFileHandler _fileHandler;

    public FileProcessor(IFileHandler fileHandler)
    {
        _fileHandler = fileHandler;
    }

    public async Task ProcessVideos()
    {
        var files = _fileHandler.GetFiles();

        await Task.CompletedTask;
    }
}
