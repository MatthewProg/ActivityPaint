using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Shared.Result;
using Mediator;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class LoadPresetCommandTests
{
    [Fact]
    public async Task Handle_WhenPathProvidedAndSucceeds_ShouldLoad()
    {
        // Arrange
        var expected = new PresetModel("Test", DateTime.Now, true, []);
        var path = @"C:\tmp\preset.json";
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock();
        var loadMock = new FileLoadServiceMock(Encoding.UTF8.GetBytes("{}", true));
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.Is<ParsePresetCommand>(x => CompareArrays(x.PresetStream.ReadBytes(), loadMock.LoadOperationBytes)),
                                       It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                    .ReturnsAsync(expected)
                    .Verifiable(Times.Once);

        var command = new LoadPresetCommand(path);
        var service = new LoadPresetCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object, mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        loadMock.Mock.Verify(x => x.GetFileStream(It.Is<string>(x => x == path)), Times.Once);
        mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenPathProvidedAndFails_ShouldFail()
    {
        // Arrange
        var path = @"C:\tmp\preset.json";
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock();
        var loadMock = new FileLoadServiceMock(shouldFail: true);
        var mediatorMock = new Mock<IMediator>();

        var command = new LoadPresetCommand(path);
        var service = new LoadPresetCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object, mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        loadMock.Mock.Verify(x => x.GetFileStream(It.Is<string>(x => x == path)), Times.Once);
        mediatorMock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenPathNotProvidedAndSucceeds_ShouldLoad()
    {
        // Arrange
        var expected = new PresetModel("Test", DateTime.Now, true, []);
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock(Encoding.UTF8.GetBytes("{}", true));
        var loadMock = new FileLoadServiceMock();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.Is<ParsePresetCommand>(x => CompareArrays(x.PresetStream.ReadBytes(), interactionMock.LoadOperationBytes)),
                                       It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                    .ReturnsAsync(expected)
                    .Verifiable(Times.Once);

        var command = new LoadPresetCommand(null);
        var service = new LoadPresetCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object, mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        loadMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileLoadAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))), Times.Once);
        mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenPathNotProvidedAndFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock(shouldFail: true);
        var loadMock = new FileLoadServiceMock();
        var mediatorMock = new Mock<IMediator>();

        var command = new LoadPresetCommand(null);
        var service = new LoadPresetCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object, mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        loadMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileLoadAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))), Times.Once);
        mediatorMock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenParseCommandFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        var interactionMock = new FileSystemInteractionMock(Encoding.UTF8.GetBytes("{}", true));
        var loadMock = new FileLoadServiceMock();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.Is<ParsePresetCommand>(x => CompareArrays(x.PresetStream.ReadBytes(), interactionMock.LoadOperationBytes)),
                               It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
            .ReturnsAsync(Error.Unknown)
            .Verifiable(Times.Once);

        var command = new LoadPresetCommand(null);
        var service = new LoadPresetCommandHandler(interactionMock.Mock.Object, loadMock.Mock.Object, mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        loadMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileLoadAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))), Times.Once);
        mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    private static bool CompareArrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length) return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i]) return false;
        }

        return true;
    }
}
