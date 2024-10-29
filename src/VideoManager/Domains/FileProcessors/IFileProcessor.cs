namespace VideoManager.Domains.FileProcessors;

public interface IFileProcessor
{
    public Task ProcessVideos(CancellationToken cancellationToken);
}
