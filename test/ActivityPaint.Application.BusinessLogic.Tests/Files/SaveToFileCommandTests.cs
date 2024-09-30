using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class SaveToFileCommandTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllDataFilled_ShouldSaveFile(bool overwrite)
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var expected = Encoding.UTF8.GetBytes("Test\ndata", true);
        using var stream = new MemoryStream(expected);
        var command = GetValidModel(stream) with { Overwrite = overwrite };

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();

        var service = new SaveToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.Verify(x => x.SaveFileAsync(It.Is<string>(x => x == command.Path),
                                                  It.IsAny<Stream>(),
                                                  It.Is<bool>(x => x == overwrite),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                             Times.Once);
        saveMock.SaveOperationBytes.Should().Equal(expected);
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Handle_WhenPathIsNullOrWhitespace_ShouldCallInteraction(string? path)
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var expected = Encoding.UTF8.GetBytes("Test\ndata", true);
        using var stream = new MemoryStream(expected);
        var command = GetValidModel(stream) with { Path = path };

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();

        var service = new SaveToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        saveMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileSaveAsync(It.Is<string>(x => x == command.SuggestedFileName),
                                                               It.IsAny<Stream>(),
                                                               It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                                    Times.Once);
        interactionMock.SaveOperationBytes.Should().Equal(expected);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSaveFails_ShouldReturnError()
    {
        // Arrange
        using var stream = new MemoryStream();
        var command = GetValidModel(stream);

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock(shouldFail: true);

        var service = new SaveToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

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
        using var stream = new MemoryStream();
        var command = GetValidModel(stream) with { Path = null };

        var interactionMock = new FileSystemInteractionMock(shouldFail: true);
        var saveMock = new FileSaveServiceMock();

        var service = new SaveToFileCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        saveMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileSaveAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                                    Times.Once);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    private static SaveToFileCommand GetValidModel(Stream data) => new(
        DataStream: data,
        SuggestedFileName: "file.txt",
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}
