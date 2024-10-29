namespace VideoManager.Domains.VideoPublishers;

public class YouTubeVideoPublisher : IVideoPublisher
{
    public async Task<bool> UploadVideoAsync(VideoFile videoFile)
    {
        await Task.CompletedTask;

        return true;
    }
}
