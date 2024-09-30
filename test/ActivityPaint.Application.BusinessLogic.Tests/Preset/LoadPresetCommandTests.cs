using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class LoadPresetCommandTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task Handle_WhenAllCorrect_ShouldLoad()
    {
        // Arrange
        var expected = new PresetModel("Test", DateTime.Now, true, []);
        var path = @"C:\tmp\preset.json";
        var cancellationToken = new CancellationToken();
        var dummyStream = new MemoryStream();

        _mediatorMock.Setup(x => x.Send(It.Is<LoadFromFileCommand>(x => x.Path == path),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(dummyStream)
                     .Verifiable(Times.Once);

        _mediatorMock.Setup(x => x.Send(It.Is<ParsePresetCommand>(x => x.PresetStream.Equals(dummyStream)),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(expected)
                     .Verifiable(Times.Once);

        var command = new LoadPresetCommand(path);
        var service = new LoadPresetCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenLoadFails_ShouldFail()
    {
        // Arrange
        var path = @"C:\tmp\preset.json";
        var cancellationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(It.Is<LoadFromFileCommand>(x => x.Path == path),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var command = new LoadPresetCommand(path);
        var service = new LoadPresetCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenParseFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var dummyStream = new MemoryStream();

        _mediatorMock.Setup(x => x.Send(It.IsAny<LoadFromFileCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(dummyStream)
                     .Verifiable(Times.Once);

        _mediatorMock.Setup(x => x.Send(It.Is<ParsePresetCommand>(x => x.PresetStream.Equals(dummyStream)),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var command = new LoadPresetCommand(null);
        var service = new LoadPresetCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }
}
