using ActivityPaint.Core.Extensions;
using ActivityPaint.Core.Shared.Result;
using ActivityPaint.Core.Shared.Result.Errors;
using Mediator;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Application.BusinessLogic.Shared.Mediator.Pipelines;

internal sealed class ExceptionHandlingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<ExceptionHandlingPipeline<TRequest, TResponse>> _logger;

    public ExceptionHandlingPipeline(ILogger<ExceptionHandlingPipeline<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return await next(message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception has occurred");

            var error = new ExceptionError(ex);

            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)Result.Failure(error);
            }

            var genericType = typeof(TResponse).GenericTypeArguments[0];
            var typedResult = typeof(Result<>).MakeGenericType(genericType)
                                              .GetMethodOfGeneric(Result.Failure)
                                              ?.Invoke(null, [error]);

            return (TResponse)typedResult!;
        }
    }
}
