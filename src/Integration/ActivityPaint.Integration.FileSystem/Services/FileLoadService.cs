﻿using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Integration.FileSystem.Services;

internal class FileLoadService : IFileLoadService
{
    private static readonly Error _emptyPathError = new("Path", "File path cannot be empty!");

    public Result<Stream> GetFileStream(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return _emptyPathError;
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return fileStream;
    }

    public async Task<Result<string>> GetFileTextAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var streamResult = GetFileStream(filePath);
        if (streamResult.IsFailure)
        {
            return streamResult.Error;
        }

        using var stream = streamResult.Value!;
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync(cancellationToken);
    }
}
