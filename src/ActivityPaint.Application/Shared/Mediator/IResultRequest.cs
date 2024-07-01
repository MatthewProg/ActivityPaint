using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Shared.Mediator;

internal interface IResultRequest : IRequest<Result>;
internal interface IResultRequest<TResponse> : IRequest<Result<TResponse>>;
