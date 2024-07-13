﻿using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Core.Shared.Result;
using ActivityPaint.Core.Shared.Result.Errors;
using CommunityToolkit.Maui.Storage;

namespace ActivityPaint.Client.Mobile.Shared.Interactions
{
    public class FileSystemInteraction : IFileSystemInteraction
    {
        private readonly IFileSaver _fileSaver;

        public FileSystemInteraction(IFileSaver fileSaver)
        {
            _fileSaver = fileSaver;
        }

        public async Task<Result> PromptFileSaveAsync(string fileName, Stream data, CancellationToken cancellationToken = default)
        {
            var saveResult = await _fileSaver.SaveAsync(fileName, data, cancellationToken);

            if (saveResult.IsSuccessful)
            {
                return Result.Success();
            }

            return new ExceptionError(saveResult.Exception);
        }
    }
}
