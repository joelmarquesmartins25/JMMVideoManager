namespace VideoManager.Domains.MetadataGenerators;

public interface IMetadataGenerator
{
    public Task<VideoFile?> GenerateMetadataAsync(string fileName);
}
