using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Image;
using ActivityPaint.Application.BusinessLogic.Image.Models;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Tests.Image;

public class SavePreviewImageCommandTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllCorrect_ShouldSaveSuccessfully(bool overwrite)
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        using var dummyStream = new MemoryStream();
        var path = @"C:\tmp\test.png";
        var modeOverwrite = ModeEnum.Dark;
        var model = new PresetModel(
            Name: "Test",
            StartDate: new(2020, 1, 1),
            IsDarkModeDefault: true,
            CanvasData: [
                IntensityEnum.Level1,
                IntensityEnum.Level0,
                IntensityEnum.Level2,
                IntensityEnum.Level0,
                IntensityEnum.Level3,
                IntensityEnum.Level4
            ]
        );
        var command = new SavePreviewImageCommand(model, modeOverwrite, path, overwrite);

        _mediatorMock.Setup(x => x.Send(It.Is<GeneratePreviewImageCommand>(x => x.Preset == command.Preset
                                                                                && x.ModeOverwrite == command.ModeOverwrite),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(dummyStream)
                     .Verifiable(Times.Once);

        _mediatorMock.Setup(x => x.Send(It.Is<SaveToFileCommand>(x => x.Overwrite == command.Overwrite
                                                                      && x.Path == command.Path
                                                                      && x.SuggestedFileName == "Test.png"
                                                                      && x.DataStream == dummyStream),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Result.Success())
                     .Verifiable(Times.Once);

        var service = new SavePreviewImageCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSaveFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        using var dummyStream = new MemoryStream();
        var model = new PresetModel(
            Name: "Test",
            StartDate: new(2020, 1, 1),
            IsDarkModeDefault: true,
            CanvasData: [
                IntensityEnum.Level1,
                IntensityEnum.Level0,
                IntensityEnum.Level2,
                IntensityEnum.Level0,
                IntensityEnum.Level3,
                IntensityEnum.Level4
            ]
        );

        var command = new SavePreviewImageCommand(model);

        _mediatorMock.Setup(x => x.Send(It.IsAny<GeneratePreviewImageCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(dummyStream)
                     .Verifiable(Times.Once);

        _mediatorMock.Setup(x => x.Send(It.IsAny<SaveToFileCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var service = new SavePreviewImageCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenGenerationFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var model = new PresetModel(
            Name: "Test",
            StartDate: new(2020, 1, 1),
            IsDarkModeDefault: true,
            CanvasData: [
                IntensityEnum.Level1,
                IntensityEnum.Level0,
                IntensityEnum.Level2,
                IntensityEnum.Level0,
                IntensityEnum.Level3,
                IntensityEnum.Level4
            ]
        );

        var command = new SavePreviewImageCommand(model);

        _mediatorMock.Setup(x => x.Send(It.IsAny<GeneratePreviewImageCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var service = new SavePreviewImageCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        _mediatorMock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }
}
