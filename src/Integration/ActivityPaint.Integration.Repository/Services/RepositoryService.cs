using ActivityPaint.Application.Abstractions.Repository;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Progress;
using ActivityPaint.Core.Shared.Result;
using LibGit2Sharp;
using System.IO.Compression;
using Repo = LibGit2Sharp.Repository;

namespace ActivityPaint.Integration.Repository.Services;

internal class RepositoryService : IRepositoryService
{
    public Result InitOrPopulateRepository(string path, AuthorModel author, IList<CommitModel> commits, Progress? progressCallback = null)
    {
        if (!Repo.IsValid(path))
        {
            _ = Repo.Init(path);
        }

        var identity = new Identity(author.FullName, author.Email);
        using var repo = new Repo(path, new() { Identity = identity });

        var commitOptions = new CommitOptions() { AllowEmptyCommit = true };
        for (var i = 0; i < commits.Count; i++)
        {
            var commit = commits[i];
            var signature = new Signature(identity, commit.DateTime);

            repo.Commit(commit.Message, signature, signature, commitOptions);
            progressCallback?.Invoke(new Status(i + 1, commits.Count));
        }

        return Result.Success();
    }

    public Result<Stream> CreateRepositoryZip(AuthorModel author, IList<CommitModel> commits, Progress? progressCallback = null)
    {
        var tmpDir = Directory.CreateTempSubdirectory("ap");
        if (tmpDir is null)
        {
            return new Error("Error.Unknown", "Unable to create temp directory!");
        }
        var path = tmpDir.FullName;

        try
        {
            var creationResult = InitOrPopulateRepository(path, author, commits, progressCallback);
            if (creationResult.IsFailure)
            {
                return creationResult.Error;
            }

            var memoryStream = new MemoryStream();
            ZipFile.CreateFromDirectory(path, memoryStream, CompressionLevel.Fastest, false);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }
        finally
        {
            foreach (var file in tmpDir.GetFiles("*", SearchOption.AllDirectories))
            {
                file.IsReadOnly = false;
            }

            tmpDir.Delete(true);
        }
    }
}
