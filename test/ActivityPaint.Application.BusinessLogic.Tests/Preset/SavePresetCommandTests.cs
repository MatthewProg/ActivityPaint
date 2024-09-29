using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class SavePresetCommandTests
{
    [Fact]
    public async Task Handle_WhenCorrectStream_ShouldDeserializeSuccessfully()
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

        var interactionMock = new FileSystemInteractionMock();
        var saveMock = new FileSaveServiceMock();

        var command = new SavePresetCommand(model, path);
        var service = new SavePresetCommandHandler(interactionMock.Mock.Object, saveMock.Mock.Object);

        // Act
        var result = await service.Handle(command, cancellationToken);

        // Assert
        interactionMock.Mock.VerifyNoOtherCalls();
        saveMock.Mock.Verify(x => x.SaveFileAsync(It.Is<string>(x => x == path),
                                                  It.IsAny<Stream>(),
                                                  It.Is<bool>(x => x == false),
                                                  It.Is<CancellationToken>(x => x.Equals(cancellationToken))),
                             Times.Once);
        saveMock.SaveOperationBytes.Should().Equal(expected);
        result.IsSuccess.Should().BeTrue();
    }
}
