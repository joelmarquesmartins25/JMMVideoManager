namespace VideoUploader.Domains;

public interface IFileHandler
{
    public IEnumerable<string> GetFiles();
}
