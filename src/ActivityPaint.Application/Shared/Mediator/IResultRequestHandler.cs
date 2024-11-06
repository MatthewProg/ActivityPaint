using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Shared.Mediator;

internal interface IResultRequestHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IResultRequest;

internal interface IResultRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IResultRequest<TResponse>;
