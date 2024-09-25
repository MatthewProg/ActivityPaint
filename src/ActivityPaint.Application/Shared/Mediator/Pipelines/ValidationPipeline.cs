using ActivityPaint.Core.Extensions;
using ActivityPaint.Core.Shared.Result;
using ActivityPaint.Core.Shared.Result.Errors;
using FluentValidation;
using Mediator;

namespace ActivityPaint.Application.BusinessLogic.Shared.Mediator.Pipelines;

internal sealed class ValidationPipeline<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (!_validators.Any())
        {
            return await next(message, cancellationToken);
        }

        var errors = await Validate(message, cancellationToken);

        return errors.Count > 0
            ? (TResponse)GetErrorResult(errors)
            : await next(message, cancellationToken);
    }

    private async Task<List<Error>> Validate(TRequest message, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(message);

        var validations = _validators.Select(x => x.ValidateAsync(context, cancellationToken));
        var validationResults = await Task.WhenAll(validations);

        return validationResults
            .Where(x => !x.IsValid)
            .SelectMany(x => x.Errors)
            .Select(x => new Error(x.PropertyName, x.ErrorMessage))
            .Distinct()
            .ToList();
    }

    private static Result GetErrorResult(IEnumerable<Error> errors)
    {
        var aggregateError = new AggregateError(errors);

        if (typeof(TResponse) == typeof(Result))
        {
            return aggregateError;
        }

        var genericType = typeof(TResponse).GenericTypeArguments[0];
        return (Result)typeof(Result<>).MakeGenericType(genericType)
                                       .GetMethodOfGeneric(Result.Failure)
                                       ?.Invoke(null, [aggregateError])!;
    }
}
