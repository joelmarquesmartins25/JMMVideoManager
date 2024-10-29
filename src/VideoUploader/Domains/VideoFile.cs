namespace VideoManager.Domains;

public record VideoFile(string FileName, string? Title = null, string? Description = null)
{
    string FileName { get; set; } = FileName;

    string? Title { get; set; } = Title;

    string? Description { get; set; } = Description;
}
