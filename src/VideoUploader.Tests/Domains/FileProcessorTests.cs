using Moq;
using VideoUploader.Domains;

namespace VideoUploader.Tests.Domains;

public class FileProcessorTests
{
    [Fact]
    public async Task ProcessVideos_SouldCallGetFiles()
    {
        // Arrange
        var fileHandlerMock = new Mock<IFileHandler>();
        var fileProcessor = new FileProcessor(fileHandlerMock.Object);

        // Act
        await fileProcessor.ProcessVideos();

        // Assert
        fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
    }
}
