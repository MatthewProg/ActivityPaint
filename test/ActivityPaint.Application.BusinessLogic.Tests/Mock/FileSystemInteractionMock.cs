using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Core.Shared.Result;
using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Mock;

public class FileSystemInteractionMock
{
    public Mock<IFileSystemInteraction> Mock { get; } = new();
    public byte[] LoadOperationBytes { get; init; } = Encoding.UTF8.GetPreamble();
    public byte[]? SaveOperationBytes { get; private set; }

    public FileSystemInteractionMock(bool shouldFail = false)
    {
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
