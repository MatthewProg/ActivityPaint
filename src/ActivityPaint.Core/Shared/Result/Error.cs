namespace ActivityPaint.Core.Shared.Result;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Unknown = new("Error.Unknown", "Unknown error has occured");
}
