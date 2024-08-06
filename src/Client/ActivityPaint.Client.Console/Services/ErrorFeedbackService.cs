using ActivityPaint.Core.Shared.Result;
using ActivityPaint.Core.Shared.Result.Errors;
using Microsoft.Extensions.Logging;

namespace ActivityPaint.Client.Console.Services;

public interface IErrorFeedbackService
{
    void WriteError(Error error);
}

internal sealed class ErrorFeedbackService : IErrorFeedbackService
{
    private readonly ILogger _logger;

    public ErrorFeedbackService(ILogger<ErrorFeedbackService> logger)
    {
        _logger = logger;
    }

    public void WriteError(Error error)
    {
        var exception = error is ExceptionError exceptionError ? exceptionError.Exception : null;
        var message = error switch
        {
            AggregateError aggregateError => GetErrorMessage(aggregateError),
            _ => error.Message
        };

        _logger.LogError(exception, message);
    }

    private static string GetErrorMessage(AggregateError error)
    {
        var message = "Multiple errors have occurred:";
        var errors = error.Errors.Select(x => $"- {x.Message} ({x.Code})");

        return string.Join('\n', [message, ..errors]);
    }
}
