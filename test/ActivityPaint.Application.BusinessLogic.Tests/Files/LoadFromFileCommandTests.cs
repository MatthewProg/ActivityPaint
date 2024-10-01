using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class LoadFromFileCommandTests
{
    [Fact]
    public async Task Handle_WhenPathProvidedAndSucceeds_ShouldLoadFile()
    {
        // Arrange
        var expected = Encoding.UTF8.GetBytes("{}", true);
        var path = @"C:\tmp\preset.json";

        var interactionMock = new FileSystemInteractionMock();
        var loadMock = new FileLoadServiceMock(expected);

        var command = new LoadFromFileCommand(path);
        var service = new LoadFromFileCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        loadMock.Mock.Verify(x => x.GetFileStream(It.Is<string>(x => x == path)), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.ReadBytes().Should().Equal(expected);
    }

    [Fact]
    public async Task Handle_WhenPathProvidedAndFails_ShouldFail()
    {
        // Arrange
        var path = @"C:\tmp\preset.json";

        var interactionMock = new FileSystemInteractionMock();
        var loadMock = new FileLoadServiceMock(shouldFail: true);

        var command = new LoadFromFileCommand(path);
        var service = new LoadFromFileCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        loadMock.Mock.Verify(x => x.GetFileStream(It.Is<string>(x => x == path)), Times.Once);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenPathNotProvidedAndSucceeds_ShouldCallInteraction()
    {
        // Arrange
        var expected = Encoding.UTF8.GetBytes("{}", true);
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock(expected);
        var loadMock = new FileLoadServiceMock();

        var command = new LoadFromFileCommand(null);
        var service = new LoadFromFileCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        loadMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileLoadAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.ReadBytes().Should().Equal(expected);
    }

    [Fact]
    public async Task Handle_WhenPathNotProvidedAndFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock(shouldFail: true);
        var loadMock = new FileLoadServiceMock();

        var command = new LoadFromFileCommand(null);
        var service = new LoadFromFileCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        loadMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileLoadAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))), Times.Once);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }
}
