using ActivityPaint.Application.Abstractions.Database;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Result;
using FluentValidation;

namespace ActivityPaint.Application.BusinessLogic.Repository;

public record UpdateRepositoryConfigCommand(
    RepositoryConfigModel Model
) : IResultRequest;

internal class UpdateRepositoryConfigCommandValidator : AbstractValidator<UpdateRepositoryConfigCommand>
{
    public UpdateRepositoryConfigCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();
    }
}

internal class UpdateRepositoryConfigCommandHandler : IResultRequestHandler<UpdateRepositoryConfigCommand>
{
    private readonly IRepositoryConfigRepository _repositoryConfigRepository;

    public UpdateRepositoryConfigCommandHandler(IRepositoryConfigRepository repositoryConfigRepository)
    {
        _repositoryConfigRepository = repositoryConfigRepository;
    }

    public async ValueTask<Result> Handle(UpdateRepositoryConfigCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model.ToRepositoryConfig();

        await _repositoryConfigRepository.UpsertFirstAsync(model, cancellationToken);

        return Result.Success();
    }
}
