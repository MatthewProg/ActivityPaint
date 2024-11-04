using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Gallery;

namespace ActivityPaint.Application.BusinessLogic.Tests.Gallery;

public class GetGalleryItemsCountCommandTests
{
    private readonly Mock<IPresetRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_WhenEverythingIsCorrect_ShouldSucceed()
    {
        // Arrange
        var expected = 99;
        var cancellationToken = new CancellationToken();

        _repositoryMock.Setup(x => x.GetCount(It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .ReturnsAsync(expected)
                       .Verifiable(Times.Once);

        var command = new GetGalleryItemsCountCommand();
        var service = new GetGalleryItemsCountCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }
}
