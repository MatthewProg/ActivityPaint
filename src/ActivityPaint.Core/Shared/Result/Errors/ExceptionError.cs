namespace ActivityPaint.Core.Shared.Result.Errors;

public sealed record ExceptionError : Error
{
    public ExceptionError(Exception exception) : base("Error.Unknown", "Unhandled exception has occured")
    {
        Exception = exception;
    }

    public Exception Exception { get; }
}
