using ActivityPaint.Integration.FileSystem.Services;
using System.Text;

namespace ActivityPaint.Integration.FileSystem.IntegrationTests.Services;

public sealed class FileLoadServiceTests : IDisposable
{
    private readonly DirectoryInfo _workingDir = Directory.CreateTempSubdirectory("ap-tests");

    [Fact]
    public async Task GetFileStream_WhenFileExists_ShouldOpenStream()
    {
        // Arrange
        var content = "Sample content\nRead test";
        var filePath = Path.Combine(_workingDir.FullName, "file.txt");

        await File.WriteAllTextAsync(filePath, content);

        var service = new FileLoadService();
        var expected = Encoding.UTF8.GetBytes(content);

        // Act
        var result = service.GetFileStream(filePath);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        using var stream = result.Value!;
        GetStreamBytes(stream).Should().Equal(expected);
    }

    [Fact]
    public void GetFileStream_WhenFileDoesNotExists_ShouldThrow()
    {
        // Arrange
        var filePath = Path.Combine(_workingDir.FullName, "file.txt");
        var service = new FileLoadService();

        // Act
        var act = () => service.GetFileStream(filePath);

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void GetFileStream_WhenFilePathEmpty_ShouldReturnError()
    {
        // Arrange
        var service = new FileLoadService();

        // Act
        var result = service.GetFileStream(string.Empty);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("empty");
    }

    [Fact]
    public async Task GetFileTextAsync_WhenFileExists_ShouldReturnText()
    {
        // Arrange
        var content = "Sample content\nRead test";
        var filePath = Path.Combine(_workingDir.FullName, "file.txt");

        await File.WriteAllTextAsync(filePath, content);

        var service = new FileLoadService();

        // Act
        var result = await service.GetFileTextAsync(filePath);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(content);
    }

    [Fact]
    public void GetFileTextAsync_WhenFileDoesNotExists_ShouldThrow()
    {
        // Arrange
        var filePath = Path.Combine(_workingDir.FullName, "file.txt");
        var service = new FileLoadService();

        // Act
        var act = async () => await service.GetFileTextAsync(filePath);

        // Assert
        act.Should().ThrowAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task GetFileTextAsync_WhenFilePathEmpty_ShouldReturnError()
    {
        // Arrange
        var service = new FileLoadService();

        // Act
        var result = await service.GetFileTextAsync(string.Empty);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("empty");
    }

    private static byte[] GetStreamBytes(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream.ToArray();
    }

    public void Dispose()
    {
        _workingDir.Delete(true);
    }
}
