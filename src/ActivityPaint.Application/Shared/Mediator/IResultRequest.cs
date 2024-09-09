using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Shared.Mediator;

public interface IResultRequest : IRequest<Result>;
public interface IResultRequest<TResponse> : IRequest<Result<TResponse>>;
