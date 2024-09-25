using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Core.Shared.Result;
using ActivityPaint.Core.Shared.Result.Errors;
using CommunityToolkit.Maui.Storage;

namespace ActivityPaint.Client.Mobile.Shared.Interactions
{
    public class FileSystemInteraction(IFileSaver fileSaver, IFilePicker filePicker) : IFileSystemInteraction
    {
        private readonly IFileSaver _fileSaver = fileSaver;
        private readonly IFilePicker _filePicker = filePicker;

        public async Task<Result> PromptFileSaveAsync(string fileName, Stream data, CancellationToken cancellationToken = default)
        {
            var saveResult = await _fileSaver.SaveAsync(fileName, data, cancellationToken);

            if (saveResult.IsSuccessful)
            {
                return Result.Success();
            }

            return new ExceptionError(saveResult.Exception);
        }

        public async Task<Result<Stream>> PromptFileLoadAsync(CancellationToken cancellationToken = default)
        {
            var fileResult = await _filePicker.PickAsync();

            if (fileResult is null)
            {
                return new Error("Error.OperationCancelled", "Operation has been cancelled by user.");
            }

            return await fileResult.OpenReadAsync();
        }
    }
}
