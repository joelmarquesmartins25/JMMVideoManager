namespace VideoUploader.Domains.VideoUploader;

public interface IVideoUploader
{
    public Task<bool> UploadVideoAsync(VideoFile videoFile);
}
