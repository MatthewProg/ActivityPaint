using ActivityPaint.Integration.FileSystem.Services;
using System.Text;

namespace ActivityPaint.Integration.FileSystem.IntegrationTests.Services;

public sealed class FileSaveServiceTests : IDisposable
{
    private readonly DirectoryInfo _workingDir = Directory.CreateTempSubdirectory("ap-tests");

    [Fact]
    public async Task SaveFileAsync_WhenFileDoesNotExit_ShouldCreate()
    {
        // Arrange
        var service = new FileSaveService();
        var savePath = Path.Combine(_workingDir.FullName, "new-file.txt");
        var content = "Test content\nLorem ipsum";
        using var data = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        var result = await service.SaveFileAsync(savePath, data, false, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        File.Exists(savePath).Should().BeTrue();
        File.ReadAllText(savePath).Should().Be(content);
    }

    [Fact]
    public async Task SaveFileAsync_WhenFileExitAndCanOverwrite_ShouldOverwrite()
    {
        // Arrange
        var service = new FileSaveService();
        var savePath = Path.Combine(_workingDir.FullName, "existing-file.txt");
        File.WriteAllText(savePath, "Sample text");

        var content = "Test content\nLorem ipsum";
        using var data = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        var result = await service.SaveFileAsync(savePath, data, true, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        File.Exists(savePath).Should().BeTrue();
        File.ReadAllText(savePath).Should().Be(content);
    }

    [Fact]
    public async Task SaveFileAsync_WhenFileExitAndCannotOverwrite_ShouldThrowException()
    {
        // Arrange
        var service = new FileSaveService();
        var savePath = Path.Combine(_workingDir.FullName, "existing-file.txt");
        var originalContent = "Sample text";
        File.WriteAllText(savePath, originalContent);

        var content = "Test content\nLorem ipsum";
        using var data = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        var act = async () => await service.SaveFileAsync(savePath, data, false, default);

        // Assert
        await act.Should().ThrowAsync<IOException>().WithMessage("*already exists*");
        File.Exists(savePath).Should().BeTrue();
        File.ReadAllText(savePath).Should().Be(originalContent);
    }

    [Fact]
    public async Task SaveFileAsync_WhenFilePathEmpty_ShouldReturnError()
    {
        // Arrange
        var service = new FileSaveService();
        using var data = new MemoryStream();

        // Act
        var result = await service.SaveFileAsync(string.Empty, data, false, default);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("empty");
    }

    public void Dispose()
    {
        _workingDir.Delete(true);
    }
}
