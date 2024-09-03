using ActivityPaint.Application.Abstractions.Database.Repositories;
using ActivityPaint.Application.BusinessLogic.Shared.Mediator;
using ActivityPaint.Application.DTOs.Repository;
using ActivityPaint.Core.Shared.Result;

namespace ActivityPaint.Application.BusinessLogic.Repository;

public record GetRepositoryConfigCommand() : IResultRequest<RepositoryConfigModel>;

internal class GetRepositoryConfigCommandHandler : IResultRequestHandler<GetRepositoryConfigCommand, RepositoryConfigModel>
{
    private readonly IRepositoryConfigRepository _repositoryConfigRepository;

    public GetRepositoryConfigCommandHandler(IRepositoryConfigRepository repositoryConfigRepository)
    {
        _repositoryConfigRepository = repositoryConfigRepository;
    }

    public async ValueTask<Result<RepositoryConfigModel>> Handle(GetRepositoryConfigCommand request, CancellationToken cancellationToken)
    {
        var config = await _repositoryConfigRepository.GetFirstAsync(cancellationToken);

        var dto = config?.ToRepositoryConfigModel();

        return dto ?? new(null, null, null);
    }
}
