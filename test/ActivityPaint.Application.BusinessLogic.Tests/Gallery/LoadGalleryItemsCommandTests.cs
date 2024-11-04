using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Gallery;
using ActivityPaint.Application.DTOs.Gallery;
using ActivityPaint.Core.Enums;
using PresetEntity = ActivityPaint.Core.Entities.Preset;

namespace ActivityPaint.Application.BusinessLogic.Tests.Gallery;

public class LoadGalleryItemsCommandTests
{
    private readonly Mock<IPresetRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_WhenEverythingIsCorrect_ShouldSucceed()
    {
        // Arrange
        var page = 2;
        var itemsCount = 10;
        var cancellationToken = new CancellationToken();
        var repoData = new List<PresetEntity>
        {
            new()
            {
                Id = 1,
                Name = "Test",
                CanvasData = [IntensityEnum.Level4],
                IsDarkModeDefault = true,
                LastUpdated = DateTimeOffset.UtcNow,
                StartDate = new DateTime(2020, 1, 1)
            }
        };

        var expected = new List<GalleryModel>
        {
            new(
                Id: repoData[0].Id,
                LastUpdated: repoData[0].LastUpdated,
                Name: repoData[0].Name,
                StartDate: repoData[0].StartDate,
                IsDarkModeDefault: repoData[0].IsDarkModeDefault,
                CanvasData: repoData[0].CanvasData
            )
        };

        _repositoryMock.Setup(x => x.GetPageAsync(It.Is<int>(x => x == page),
                                                  It.Is<int>(x => x == itemsCount),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .ReturnsAsync(repoData)
                       .Verifiable(Times.Once);

        var command = new LoadGalleryItemsCommand(page, itemsCount);
        var service = new LoadGalleryItemsCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Equal(expected);
    }
}
