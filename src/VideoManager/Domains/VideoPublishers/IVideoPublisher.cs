namespace VideoManager.Domains.VideoPublishers;

public interface IVideoPublisher
{
    public Task<bool> UploadVideoAsync(VideoFile videoFile);
}
