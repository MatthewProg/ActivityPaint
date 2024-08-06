using ActivityPaint.Application.Abstractions.FileSystem;
using ActivityPaint.Application.Abstractions.Interactions;
using ActivityPaint.Client.Console.Validators;
using ActivityPaint.Core.Shared.Result;
using Spectre.Console;

namespace ActivityPaint.Client.Console.Interactions;

internal class FileSystemInteraction : IFileSystemInteraction
{
    private readonly IFileSaveService _fileSaveService;

    public FileSystemInteraction(IFileSaveService fileSaveService)
    {
        _fileSaveService = fileSaveService;
    }

    public async Task<Result> PromptFileSaveAsync(string fileName, Stream data, CancellationToken cancellationToken = default)
    {
        var savePrompt = new TextPrompt<string>("Please enter file save path")
            .DefaultValue(fileName)
            .Validate(value =>
            {
                _ = OptionsValidator.ValidatePath(value, "Save Path", out var result);
                return result;
            });

        var filePath = AnsiConsole.Prompt(savePrompt);

        var overwrite = false;
        if (File.Exists(filePath))
        {
            overwrite = AnsiConsole.Confirm("The file already exists. Do you want to overwrite it?", false);
            if (!overwrite)
            {
                return Result.Success();
            }
        }

        return await _fileSaveService.SaveFileAsync(filePath, data, overwrite, cancellationToken);
    }
}
