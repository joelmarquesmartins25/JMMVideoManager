namespace VideoUploader.Domains.FileHandler;

public interface IFileHandler
{
    public IEnumerable<string> GetFiles();

    public void HandleFailure(string filePath);

    public void HandleSuccess(string filePath);
}
