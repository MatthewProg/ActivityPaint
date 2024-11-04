using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Gallery;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Gallery;

public class SaveGalleryItemCommandTests
{
    private readonly Mock<IPresetRepository> _repositoryMock = new();
    private readonly Mock<TimeProvider> _timeProviderMock = new();

    [Fact]
    public async Task Handle_WhenEverythingIsCorrect_ShouldSucceed()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var model = new PresetModel("Test", DateTime.Now, true, [IntensityEnum.Level1]);
        var expectedTime = new DateTimeOffset(2020, 10, 10, 10, 10, 10, 10, TimeSpan.FromHours(1));

        _timeProviderMock.Setup(x => x.GetUtcNow())
                         .Returns(expectedTime)
                         .Verifiable(Times.Once);

        _repositoryMock.Setup(x => x.InsertAsync(It.Is<PresetEntity>(x => x.Name == model.Name
                                                                          && x.StartDate == model.StartDate
                                                                          && x.CanvasData == model.CanvasData
                                                                          && x.IsDarkModeDefault == model.IsDarkModeDefault
                                                                          && x.LastUpdated == expectedTime),
                                                 It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .Verifiable(Times.Once);

        var command = new SaveGalleryItemCommand(model);
        var service = new SaveGalleryItemCommandHandler(_repositoryMock.Object, _timeProviderMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        _timeProviderMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }
}
