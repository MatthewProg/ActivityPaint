using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Core.Shared.Result;
using Mediator;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Files;

public class SaveTextToFileCommandTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllCorrect_ShouldSaveFile(bool overwrite)
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var command = GetValidModel() with { Overwrite = overwrite };
        var bytes = Encoding.UTF8.GetBytes(command.Text, true);

        _mediatorMock.Setup(x => x.Send(It.Is<SaveToFileCommand>(x => x.Overwrite == command.Overwrite
                                                                      && x.Path == command.Path
                                                                      && x.SuggestedFileName == "save.txt"
                                                                      && x.DataStream.ReadBytes().SequenceEqual(bytes)),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Result.Success())
                     .Verifiable(Times.Once);

        var service = new SaveTextToFileCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSaveFails_ShouldReturnError()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var command = GetValidModel();

        _mediatorMock.Setup(x => x.Send(It.IsAny<SaveToFileCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var service = new SaveTextToFileCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }

    private static SaveTextToFileCommand GetValidModel() => new(
        Text: "Sample\nMultiline\nContent",
        Path: @"C:\Temp\file.txt",
        Overwrite: true
    );
}
