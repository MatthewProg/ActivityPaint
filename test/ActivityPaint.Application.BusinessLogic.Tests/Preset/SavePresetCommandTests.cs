using ActivityPaint.Application.BusinessLogic.Files;
using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using ActivityPaint.Core.Shared.Result;
using Mediator;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class SavePresetCommandTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenAllCorrect_ShouldSerializeSuccessfully(bool overwrite)
    {
        // Arrange
        var serialized = """{"Name":"Test","StartDate":"2020-01-01T00:00:00","IsDarkModeDefault":true,"CanvasData":"eAFiZGBiYGYBAAAA//8="}""";
        var expected = Encoding.UTF8.GetBytes(serialized);
        var cancellationToken = new CancellationToken();
        var path = @"C:\test\file.json";
        var model = new PresetModel(
            Name: "Test",
            StartDate: new(2020, 1, 1),
            IsDarkModeDefault: true,
            CanvasData: [
                IntensityEnum.Level1,
                IntensityEnum.Level0,
                IntensityEnum.Level2,
                IntensityEnum.Level0,
                IntensityEnum.Level3,
                IntensityEnum.Level4
            ]
        );
        var command = new SavePresetCommand(model, path, overwrite);

        _mediatorMock.Setup(x => x.Send(It.Is<SaveToFileCommand>(x => x.Overwrite == command.Overwrite
                                                                      && x.Path == command.Path
                                                                      && x.SuggestedFileName == "Test.json"
                                                                      && x.DataStream.ReadBytes().SequenceEqual(expected)),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Result.Success())
                     .Verifiable(Times.Once);

        var service = new SavePresetCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSaveFails_ShouldFail()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var path = @"C:\test\file.json";
        var model = new PresetModel(
            Name: "Test",
            StartDate: new(2020, 1, 1),
            IsDarkModeDefault: true,
            CanvasData: [
                IntensityEnum.Level1,
                IntensityEnum.Level0,
                IntensityEnum.Level2,
                IntensityEnum.Level0,
                IntensityEnum.Level3,
                IntensityEnum.Level4
            ]
        );
        var command = new SavePresetCommand(model, path);

        _mediatorMock.Setup(x => x.Send(It.IsAny<SaveToFileCommand>(),
                                        It.Is<CancellationToken>(x => x.Equals(cancellationToken))))
                     .ReturnsAsync(Error.Unknown)
                     .Verifiable(Times.Once);

        var service = new SavePresetCommandHandler(_mediatorMock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        _mediatorMock.VerifyAll();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.Unknown);
    }
}
