namespace VideoUploader.Domains.MetadataGenerator;

public interface IMetadataGenerator
{
    public Task<VideoFile?> GenerateMetadataAsync(string fileName);
}
