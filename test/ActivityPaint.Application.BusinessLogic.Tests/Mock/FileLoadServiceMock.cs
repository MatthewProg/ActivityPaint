using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Mock;

public class FileLoadServiceMock
{
    public readonly Mock<IFileLoadService> Mock = new();
    public readonly byte[] LoadOperationBytes;

    public FileLoadServiceMock(byte[]? loadOperationBytes = null, bool shouldFail = false)
    {
        LoadOperationBytes = loadOperationBytes ?? Encoding.UTF8.GetPreamble();

        Mock.Setup(x => x.GetFileStream(It.IsAny<string>()))
            .Returns(shouldFail ? Error.Unknown : new MemoryStream(LoadOperationBytes));

        Mock.Setup(x => x.GetFileTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(shouldFail ? Error.Unknown : Encoding.UTF8.GetString(LoadOperationBytes));
    }
}
