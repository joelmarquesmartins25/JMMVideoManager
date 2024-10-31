using Microsoft.Extensions.Logging;
using Moq;

namespace VideoManager.Tests.Helpers;

public static class LoggerExtensions
{
    public static void VerifyLogCount<T>(
        this Mock<ILogger<T>> loggerMock,
        Times times)
    {
        loggerMock.Verify(
            logger => logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    public static void VerifyLogCount<T>(
        this Mock<ILogger<T>> loggerMock,
        Func<Times> times)
    {
        loggerMock.Verify(
            logger => logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    public static void VerifyLogLevelCount<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel logLevel,
        Times times)
    {
        loggerMock.Verify(
            logger => logger.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    public static void VerifyLogLevelCount<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel logLevel,
        Func<Times> times)
    {
        loggerMock.Verify(
            logger => logger.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }

    public static void VerifyLogLevelContainsMessage<T>(
        this Mock<ILogger<T>> loggerMock,
        LogLevel logLevel,
        string expectedMessage,
        Func<Times> times)
    {
        loggerMock.Verify(
            logger => logger.Log(
                logLevel,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            times);
    }
}
