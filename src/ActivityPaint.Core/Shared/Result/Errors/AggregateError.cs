namespace ActivityPaint.Core.Shared.Result.Errors;

public sealed record AggregateError(IEnumerable<Error> Errors)
    : Error("Error.Multiple", "Multiple errors");
