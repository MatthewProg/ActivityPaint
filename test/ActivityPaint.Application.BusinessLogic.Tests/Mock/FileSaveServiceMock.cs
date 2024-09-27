using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Tests.Mock;

public class FileSaveServiceMock
{
    public Mock<IFileSaveService> Mock { get; } = new();
    public byte[]? SaveOperationBytes { get; private set; }

    public FileSaveServiceMock(bool shouldFail = false)
    {
        Mock.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, Stream stream, bool _, CancellationToken _) =>
            {
                SaveOperationBytes = stream.ReadBytes();

                return shouldFail
                   ? Error.Unknown
                   : Result.Success();
            });
    }
}
