namespace ActivityPaint.Core.Shared.Result.Errors;

public sealed record AggregateError : Error
{
    public AggregateError(IEnumerable<Error> errors) : base("Error.Multiple", "Multiple errors")
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get; }
}
