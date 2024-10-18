using ActivityPaint.Application.BusinessLogic.Image;
using ActivityPaint.Application.BusinessLogic.Image.Services;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;

namespace ActivityPaint.Application.BusinessLogic.Tests.Image;

public class GeneratePreviewImageCommandTests
{
    private readonly Mock<IPreviewImageService> _previewImageServiceMock = new();

    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllCorrect_ShouldSucceed(bool? darkModeOverwrite)
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

        var command = new GeneratePreviewImageCommand(model, darkModeOverwrite);

        _previewImageServiceMock.Setup(x => x.GeneratePreviewAsync(It.Is<PresetModel>(x => x == command.Preset),
                                                                   It.Is<bool?>(x => x == command.DarkModeOverwrite),
                                                                   It.Is<CancellationToken>(x => x == cancellationToken)))
                                .ReturnsAsync(dummyStream)
                                .Verifiable(Times.Once);


        var service = new GeneratePreviewImageCommandHandler(_previewImageServiceMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _previewImageServiceMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenGenerationFails_ShouldFail()
    {
        // Arrange
        var exception = new InsufficientMemoryException();
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

        var command = new GeneratePreviewImageCommand(model);

        _previewImageServiceMock.Setup(x => x.GeneratePreviewAsync(It.IsAny<PresetModel>(),
                                                                   It.IsAny<bool?>(),
                                                                   It.IsAny<CancellationToken>()))
                                .ThrowsAsync(exception)
                                .Verifiable(Times.Once);


        var service = new GeneratePreviewImageCommandHandler(_previewImageServiceMock.Object);
        var action = async () => await service.Handle(command, default);

        // Act & Assert
        await action.Should().ThrowAsync<InsufficientMemoryException>();
        _previewImageServiceMock.VerifyAll();
    }
}
