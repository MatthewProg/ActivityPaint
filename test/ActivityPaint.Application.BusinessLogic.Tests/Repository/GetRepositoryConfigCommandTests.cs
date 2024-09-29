using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Repository;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Entities;

namespace ActivityPaint.Application.BusinessLogic.Tests.Repository;

public class GetRepositoryConfigCommandTests
{
    [Fact]
    public async Task Handle_WhenNoConfigs_ShouldReturnModelWithNulls()
    {
        // Arrange
        var expected = new RepositoryConfigModel(null, null, null);
        var cancellationToken = new CancellationToken();
        var mock = new Mock<IRepositoryConfigRepository>();
        mock.Setup(x => x.GetFirstAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
            .ReturnsAsync((RepositoryConfig)null!)
            .Verifiable(Times.Once);

        var command = new GetRepositoryConfigCommand();
        var service = new GetRepositoryConfigCommandHandler(mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenGotConfigs_ShouldReturnFirst()
    {
        // Arrange
        var expected = new RepositoryConfigModel("{name} commit", "test@example.com", "Unit Test");
        var cancellationToken = new CancellationToken();
        var mock = new Mock<IRepositoryConfigRepository>();
        mock.Setup(x => x.GetFirstAsync(It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
            .ReturnsAsync(new RepositoryConfig()
            {
                Id = 1,
                AuthorEmail = "test@example.com",
                AuthorFullName = "Unit Test",
                MessageFormat = "{name} commit"
            })
            .Verifiable(Times.Once);

        var command = new GetRepositoryConfigCommand();
        var service = new GetRepositoryConfigCommandHandler(mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }
}
