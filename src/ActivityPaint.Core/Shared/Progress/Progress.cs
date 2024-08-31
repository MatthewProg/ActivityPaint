namespace ActivityPaint.Core.Shared.Progress;

public record struct Status(int Current, int Count)
{
    public readonly decimal Progress
        => decimal.Divide(Current, Count);
}

public delegate void Progress(Status status);
