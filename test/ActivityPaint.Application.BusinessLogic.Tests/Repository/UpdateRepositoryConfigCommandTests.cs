using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Repository;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.BusinessLogic.Tests.Repository;

public class UpdateRepositoryConfigCommandTests
{
    private readonly Mock<IRepositoryConfigRepository> _repositoryMock = new();

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

        _repositoryMock.Setup(x => x.UpsertFirstAsync(It.Is<RepositoryConfig>(x => CompareRepositoryConfig(x, expected)),
                                                      It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .Verifiable(Times.Once);

        var command = new UpdateRepositoryConfigCommand(model);
        var service = new UpdateRepositoryConfigCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenEmptyString_ShouldUpdateAsNull()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var model = new RepositoryConfigModel(string.Empty, string.Empty, string.Empty);

        _repositoryMock.Setup(x => x.UpsertFirstAsync(It.Is<RepositoryConfig>(x => CompareRepositoryConfig(x, new())),
                                                      It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                       .Verifiable(Times.Once);

        var command = new UpdateRepositoryConfigCommand(model);
        var service = new UpdateRepositoryConfigCommandHandler(_repositoryMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    private static bool CompareRepositoryConfig(RepositoryConfig left, RepositoryConfig right)
        => left.AuthorEmail == right.AuthorEmail
        && left.AuthorFullName == right.AuthorFullName
        && left.MessageFormat == right.MessageFormat;
}
