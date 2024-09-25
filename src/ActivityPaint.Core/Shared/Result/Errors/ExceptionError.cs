namespace ActivityPaint.Core.Shared.Result.Errors;

public sealed record ExceptionError(Exception Exception)
    : Error("Error.Unknown", "Unhandled exception has occurred");
