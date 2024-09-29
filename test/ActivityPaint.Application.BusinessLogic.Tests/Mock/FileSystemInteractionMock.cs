using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Mock;

public class FileSystemInteractionMock
{
    public readonly Mock<IFileSystemInteraction> Mock = new();
    public readonly byte[] LoadOperationBytes;
    public byte[]? SaveOperationBytes { get; private set; }

    public FileSystemInteractionMock(byte[]? loadOperationBytes = null, bool shouldFail = false)
    {
        LoadOperationBytes = loadOperationBytes ?? Encoding.UTF8.GetPreamble();

        Mock.Setup(x => x.PromptFileSaveAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, Stream stream, CancellationToken _) =>
            {
                SaveOperationBytes = stream.ReadBytes();

                return shouldFail
                   ? Error.Unknown
                   : Result.Success();
            });

        Mock.Setup(x => x.PromptFileLoadAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(shouldFail ? Error.Unknown : new MemoryStream(LoadOperationBytes));
    }
}
