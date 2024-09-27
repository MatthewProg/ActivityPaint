using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class SaveTextToFileCommandTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllDataFilled_ShouldSaveFile(bool overwrite)
    {
        // Arrange
        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        var command = GetValidModel() with { Overwrite = overwrite };
        byte[] contentBytes = Encoding.UTF8.GetBytes(command.Text, true);
        var service = new SaveTextToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.Verify(x => x.SaveFileAsync(It.Is(command.Path!, StringComparer.Ordinal),
                                                  It.IsAny<Stream>(),
                                                  It.Is<bool>(x => x == overwrite),
                                                  It.IsAny<CancellationToken>()),
                             Times.Once);
        result.IsSuccess.Should().BeTrue();
        saveMock.SaveOperationBytes.Should().BeEquivalentTo(contentBytes);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Handle_WhenPathIsNullOrWhitespace_ShouldCallInteraction(string? path)
    {
        // Arrange
        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        var command = GetValidModel() with { Path = path };
        byte[] contentBytes = Encoding.UTF8.GetBytes(command.Text, true);
        var service = new SaveTextToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        saveMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileSaveAsync(It.Is("save.txt", StringComparer.Ordinal),
                                                               It.IsAny<Stream>(),
                                                               It.IsAny<CancellationToken>()),
                                    Times.Once);
        result.IsSuccess.Should().BeTrue();
        interactionMock.SaveOperationBytes.Should().BeEquivalentTo(contentBytes);
    }

    [Fact]
    public async Task Handle_WhenSaveFails_ShouldReturnError()
    {
        // Arrange
        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock(true);
        var command = GetValidModel();
        var service = new SaveTextToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.Verify(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
                             Times.Once);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenInteractionFails_ShouldReturnError()
    {
        // Arrange
        var interactionMock = new FileSystemInteractionMock(true);
        var saveMock = new FileSaveServiceMock();
        var command = GetValidModel() with { Path = null };
        var service = new SaveTextToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        saveMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileSaveAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                                    Times.Once);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    private static SaveTextToFileCommand GetValidModel() => new(
        Text: "Sample\nMultiline\nContent",
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}
