namespace VideoManager.Domains.MetadataGenerators;

public class MetadataGenerator : IMetadataGenerator
{
    public async Task<VideoFile?> GenerateMetadataAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var videoFile = new VideoFile(fileName, "Title", "Brief Description");

        await Task.CompletedTask;

        return videoFile;
    }
}
