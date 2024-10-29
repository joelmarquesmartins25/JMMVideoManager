using Microsoft.Extensions.Logging;
using Moq;
using VideoUploader.Domains;
using VideoUploader.Domains.FileHandler;
using VideoUploader.Domains.MetadataGenerator;
using VideoUploader.Domains.VideoUploader;
using VideoUploader.Helpers;
using VideoUploader.Tests.Helpers;

namespace VideoUploader.Tests.Domains;

public class FileProcessorTests
{
    private const string _fileName = "video1.mp4";
    private readonly VideoFile _videoFile;
    private readonly List<string> _files = [];

    private readonly Mock<IFileHandler> _fileHandlerMock;
    private readonly Mock<IMetadataGenerator> _metadataGeneratorMock;
    private readonly Mock<IVideoUploader> _videoUploaderMock;
    private readonly Mock<ILogger<FileProcessor>> _loggerMock;
    private readonly FileProcessor _fileProcessor;

    public FileProcessorTests()
    {
        _files.Add(_fileName);
        _videoFile = new VideoFile(_fileName, "Title", "Brief Description");

        _fileHandlerMock = new Mock<IFileHandler>();
        _metadataGeneratorMock = new Mock<IMetadataGenerator>();
        _videoUploaderMock = new Mock<IVideoUploader>();
        _loggerMock = new Mock<ILogger<FileProcessor>>();

        _fileProcessor = new FileProcessor(_fileHandlerMock.Object, _metadataGeneratorMock.Object, _videoUploaderMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ProcessVideos_NoFilesFound_ShouldNotCallOtherMethods()
    {
        // Arrange
        _fileHandlerMock.Setup(fh => fh.GetFiles()).Returns(Array.Empty<string>);

        // Act
        await _fileProcessor.ProcessVideos();

        // Assert
        _fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleFailure(_fileName), Times.Never);
        _fileHandlerMock.Verify(fh => fh.HandleSuccess(_fileName), Times.Never);

        _metadataGeneratorMock.Verify(mg => mg.GenerateMetadataAsync(_fileName), Times.Never);

        _videoUploaderMock.Verify(vu => vu.UploadVideoAsync(_videoFile), Times.Never);

        _loggerMock.VerifyLog(Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, Times.Once);
        _loggerMock.VerifyLog(LogLevel.Information, LogMessages.NoFilesFound, Times.Once);
    }

    [Fact]
    public async Task ProcessVideos_FilesFound_ShouldCallGenerateMetadata()
    {
        // Arrange
        _fileHandlerMock.Setup(fh => fh.GetFiles()).Returns(_files);

        // Act
        await _fileProcessor.ProcessVideos();

        // Assert
        _fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
        _metadataGeneratorMock.Verify(mg => mg.GenerateMetadataAsync(_fileName), Times.Once);
    }

    [Fact]
    public async Task ProcessVideos_FilesFound_GenerateMetadataNull_ShouldCallHandleError()
    {
        // Arrange
        _fileHandlerMock.Setup(fh => fh.GetFiles()).Returns(_files);
        _metadataGeneratorMock.Setup(mg => mg.GenerateMetadataAsync(_fileName)).ReturnsAsync((VideoFile?)null);

        // Act
        await _fileProcessor.ProcessVideos();

        // Assert
        _fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleFailure(_fileName), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleSuccess(_fileName), Times.Never);

        _metadataGeneratorMock.Verify(mg => mg.GenerateMetadataAsync(_fileName), Times.Once);

        _videoUploaderMock.Verify(vu => vu.UploadVideoAsync(It.IsAny<VideoFile>()), Times.Never);

        _loggerMock.VerifyLog(Times.Exactly(2));
        _loggerMock.VerifyLog(LogLevel.Warning, Times.Once);
        _loggerMock.VerifyLog(LogLevel.Warning, LogMessages.FailedToGenerateMetadata, Times.Once);
    }

    [Fact]
    public async Task ProcessVideos_FilesFound_UploadVideoFails_ShouldCallHandleError()
    {
        // Arrange
        _fileHandlerMock.Setup(fh => fh.GetFiles()).Returns(_files);
        _metadataGeneratorMock.Setup(mg => mg.GenerateMetadataAsync(_fileName)).ReturnsAsync(_videoFile);
        _videoUploaderMock.Setup(vu => vu.UploadVideoAsync(_videoFile)).ReturnsAsync(false);

        // Act
        await _fileProcessor.ProcessVideos();

        // Assert
        _fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleFailure(_fileName), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleSuccess(_fileName), Times.Never);

        _metadataGeneratorMock.Verify(mg => mg.GenerateMetadataAsync(_fileName), Times.Once);

        _videoUploaderMock.Verify(vu => vu.UploadVideoAsync(_videoFile), Times.Once);

        _loggerMock.VerifyLog(Times.Exactly(2));
        _loggerMock.VerifyLog(LogLevel.Warning, Times.Once);
        _loggerMock.VerifyLog(LogLevel.Warning, LogMessages.FailedToUploadVideo, Times.Once);
    }

    [Fact]
    public async Task ProcessVideos_FilesFound_UploadVideoSuccess_ShouldCallHandleSuccess()
    {
        // Arrange
        _fileHandlerMock.Setup(fh => fh.GetFiles()).Returns(_files);
        _metadataGeneratorMock.Setup(mg => mg.GenerateMetadataAsync(_fileName)).ReturnsAsync(_videoFile);
        _videoUploaderMock.Setup(vu => vu.UploadVideoAsync(_videoFile)).ReturnsAsync(true);

        // Act
        await _fileProcessor.ProcessVideos();

        // Assert
        _fileHandlerMock.Verify(fh => fh.GetFiles(), Times.Once);
        _fileHandlerMock.Verify(fh => fh.HandleFailure(_fileName), Times.Never);
        _fileHandlerMock.Verify(fh => fh.HandleSuccess(_fileName), Times.Once);

        _metadataGeneratorMock.Verify(mg => mg.GenerateMetadataAsync(_fileName), Times.Once);

        _videoUploaderMock.Verify(vu => vu.UploadVideoAsync(_videoFile), Times.Once);

        _loggerMock.VerifyLog(Times.Exactly(2));
        _loggerMock.VerifyLog(LogLevel.Information, Times.Exactly(2));
        _loggerMock.VerifyLog(LogLevel.Information, LogMessages.SuccessfullyProcessedFile, Times.Once);
    }
}
