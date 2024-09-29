using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.BusinessLogic.Generate;
using ActivityPaint.Application.BusinessLogic.Generate.Services;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Tests.Generate;

public class GenerateRepoCommandTests
{
    private static readonly List<CommitModel> _commits =
    [
        new("Name commit #1", new DateTimeOffset(2020, 1, 1, 12, 0, 0, TimeSpan.FromHours(1))),
        new("Name commit #2", new DateTimeOffset(2020, 1, 2, 12, 0, 0, TimeSpan.FromHours(1))),
        new("Name commit #3", new DateTimeOffset(2020, 1, 2, 12, 0, 0, TimeSpan.FromHours(1)))
    ];

    private readonly Mock<ICommitsService> _commitsServiceMock = new();
    private readonly Mock<IRepositoryService> _repositoryServiceMock = new();

    public GenerateRepoCommandTests()
    {
        _commitsServiceMock.Setup(x => x.GenerateCommits(It.IsAny<PresetModel>(), It.IsAny<string?>()))
                           .Returns(_commits)
                           .Verifiable(Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNotZipAndAllCorrect_ShouldBeValid()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var command = new GenerateRepoCommand(preset, author, false, Path: @"C:\tmp\repo");

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        _repositoryServiceMock.Setup(x => x.InitOrPopulateRepository(It.Is<string>(x => x == command.Path),
                                                                     It.Is<AuthorModel>(x => x.Equals(author)),
                                                                     It.Is<IList<CommitModel>>(x => x.Equals(_commits)),
                                                                     It.IsAny<Progress>()))
                              .Returns(Result.Success())
                              .Verifiable(Times.Once);

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyAll();
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.VerifyNoOtherCalls();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenNotZipAndRepositoryFails_ShouldFail()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var command = new GenerateRepoCommand(preset, author, false, Path: @"C:\tmp\repo");

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        _repositoryServiceMock.Setup(x => x.InitOrPopulateRepository(It.Is<string>(x => x == command.Path),
                                                                     It.Is<AuthorModel>(x => x.Equals(author)),
                                                                     It.Is<IList<CommitModel>>(x => x.Equals(_commits)),
                                                                     It.IsAny<Progress>()))
                              .Returns(Error.Unknown)
                              .Verifiable(Times.Once);

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyAll();
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Fact]
    public async Task Handle_WhenNotZipAndPathEmpty_ShouldReturnError()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var command = new GenerateRepoCommand(preset, author, false, Path: null);

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyNoOtherCalls();
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("path");
    }

    [Fact]
    public async Task Handle_WhenZipAndRepositoryCreationFails_ShouldFail()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var path = @"C:\tmp\test.zip";
        var command = new GenerateRepoCommand(preset, author, true, path);

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        _repositoryServiceMock.Setup(x => x.CreateRepositoryZip(It.Is<AuthorModel>(x => x.Equals(author)),
                                                                It.Is<IList<CommitModel>>(x => x.Equals(_commits)),
                                                                It.IsAny<Progress>()))
                              .Returns(Error.Unknown)
                              .Verifiable(Times.Once);

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, default);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyAll();
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.VerifyNoOtherCalls();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenZipAndPathValid_ShouldBeValid(bool overwrite)
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var path = @"C:\tmp\test.zip";
        var cancellationToken = new CancellationToken();
        byte[] dummyBytes = [0x50, 0x00, 0xC0, 0x01];
        using var dummyStream = new MemoryStream(dummyBytes);
        var command = new GenerateRepoCommand(preset, author, true, path, overwrite);

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        _repositoryServiceMock.Setup(x => x.CreateRepositoryZip(It.Is<AuthorModel>(x => x.Equals(author)),
                                                                It.Is<IList<CommitModel>>(x => x.Equals(_commits)),
                                                                It.IsAny<Progress>()))
                              .Returns(dummyStream)
                              .Verifiable(Times.Once);

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyAll();
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.Verify(x => x.SaveFileAsync(It.Is<string>(x => x == path),
                                                  It.Is<Stream>(x => x.Equals(dummyStream)),
                                                  It.Is<bool>(x => x == overwrite),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                             Times.Once);
        saveMock.SaveOperationBytes.Should().Equal(dummyBytes);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenZipAndPathNull_ShouldCallInteraction()
    {
        // Arrange
        var preset = GetValidPreset();
        var author = GetValidAuthor();
        var cancellationToken = new CancellationToken();
        byte[] dummyBytes = [0x50, 0x00, 0xC0, 0x01];
        using var dummyStream = new MemoryStream(dummyBytes);
        var command = new GenerateRepoCommand(preset, author, true);

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();
        _repositoryServiceMock.Setup(x => x.CreateRepositoryZip(It.Is<AuthorModel>(x => x.Equals(author)),
                                                                It.Is<IList<CommitModel>>(x => x.Equals(_commits)),
                                                                It.IsAny<Progress>()))
                              .Returns(dummyStream)
                              .Verifiable(Times.Once);

        var service = new GenerateRepoCommandHandler(
            interactionMock.Mock.Object,
            _repositoryServiceMock.Object,
            saveMock.Mock.Object,
            _commitsServiceMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _commitsServiceMock.VerifyAll();
        _repositoryServiceMock.VerifyAll();
        saveMock.Mock.VerifyNoOtherCalls();
        interactionMock.Mock.Verify(x => x.PromptFileSaveAsync(It.Is<string>(x => x == $"Name_.zip"),
                                                  It.Is<Stream>(x => x.Equals(dummyStream)),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                             Times.Once);
        interactionMock.SaveOperationBytes.Should().Equal(dummyBytes);
        result.IsSuccess.Should().BeTrue();
    }

    private static PresetModel GetValidPreset() => new(
        Name: "Name?",
        StartDate: new DateTime(2020, 1, 1),
        IsDarkModeDefault: true,
        CanvasData: [IntensityEnum.Level1, IntensityEnum.Level2]
    );

    private static AuthorModel GetValidAuthor() => new(
        Email: "email@example.com",
        FullName: "John Doe"
    );
}
