using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Gallery;

namespace ActivityPaint.Application.BusinessLogic.Tests.Gallery;

public class DeleteGalleryItemCommandTests
{
    private readonly Mock<IPresetRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_WhenEverythingIsCorrect_ShouldSucceed()
    {
        // Arrange
        var deleteId = 9;
        var cancellationToken = new CancellationToken();

        _repositoryMock.Setup(x => x.DeleteAsync(It.Is<int>(x => x == deleteId),
                                                 It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .Verifiable(Times.Once);

        var command = new DeleteGalleryItemCommand(deleteId);
        var service = new DeleteGalleryItemCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }
}
