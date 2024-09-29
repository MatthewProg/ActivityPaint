using ActivityPaint.Application.BusinessLogic.Preset;
using ActivityPaint.Application.BusinessLogic.Tests.Mock;
using ActivityPaint.Application.DTOs.Preset;
using ActivityPaint.Core.Enums;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Preset;

public class ParsePresetCommandTests
{
    [Fact]
    public async Task Handle_WhenCorrectStream_ShouldDeserializeSuccessfully()
    {
        // Arrange
        var expected = new PresetModel(
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
        var data = """{"Name":"Test","StartDate":"2020-01-01T00:00:00","IsDarkModeDefault":true,"CanvasData":"eAFiZGBiYGYBAAAA//8="}""";
        var bytes = Encoding.UTF8.GetBytes(data, true);
        using var stream = new MemoryStream(bytes);

        var command = new ParsePresetCommand(stream);
        var service = new ParsePresetCommandHandler();

        // Act
        var result = await service.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }
}
