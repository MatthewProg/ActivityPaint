using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Repository;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.BusinessLogic.Tests.Repository;

public class UpdateRepositoryConfigCommandTests
{
    [Fact]
    public async Task Handle_WhenAllHaveValues_ShouldMapCorrectly()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var model = new RepositoryConfigModel("Format", "test@example.com", "Author Name");
        var expected = new RepositoryConfig()
        {
            AuthorEmail = "test@example.com",
            AuthorFullName = "Author Name",
            MessageFormat = "Format"
        };
        var mock = new Mock<IRepositoryConfigRepository>();
        mock.Setup(x => x.UpsertFirstAsync(It.IsAny<RepositoryConfig>(), It.IsAny<CancellationToken>()));

        var command = new UpdateRepositoryConfigCommand(model);
        var service = new UpdateRepositoryConfigCommandHandler(mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        mock.Verify(x => x.UpsertFirstAsync(It.Is<RepositoryConfig>(x => CompareRepositoryConfig(x, expected)),
                                            It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                    Times.Once);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenEmptyString_ShouldUpdateAsNull()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var model = new RepositoryConfigModel(string.Empty, string.Empty, string.Empty);
        var mock = new Mock<IRepositoryConfigRepository>();
        mock.Setup(x => x.UpsertFirstAsync(It.IsAny<RepositoryConfig>(), It.IsAny<CancellationToken>()));

        var command = new UpdateRepositoryConfigCommand(model);
        var service = new UpdateRepositoryConfigCommandHandler(mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        mock.Verify(x => x.UpsertFirstAsync(It.Is<RepositoryConfig>(x => CompareRepositoryConfig(x, new())),
                                            It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                    Times.Once);
        result.IsSuccess.Should().BeTrue();
    }

    private static bool CompareRepositoryConfig(RepositoryConfig left, RepositoryConfig right)
        => left.AuthorEmail == right.AuthorEmail
        && left.AuthorFullName == right.AuthorFullName
        && left.MessageFormat == right.MessageFormat;
}
