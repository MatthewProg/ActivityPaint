using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Gallery;
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

        var expected = new List<PresetEntity>
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

        _repositoryMock.Setup(x => x.GetPageAsync(It.Is<int>(x => x == page),
                                                  It.Is<int>(x => x == itemsCount),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .ReturnsAsync(expected)
                       .Verifiable(Times.Once);

        var command = new LoadGalleryItemsCommand(page, itemsCount);
        var service = new LoadGalleryItemsCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }
}
