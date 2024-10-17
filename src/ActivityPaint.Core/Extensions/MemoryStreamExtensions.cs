namespace ActivityPaint.Core.Extensions;

public static class MemoryStreamExtensions
{
    public static ArraySegment<byte> AsArraySegment(this MemoryStream stream)
    {
        if (stream.TryGetBuffer(out var buffer))
        {
            return buffer;
        }

        var array = stream.ToArray();
        return new(array);
    }

    public static string ToBase64String(this MemoryStream stream)
    {
        var array = stream.AsArraySegment();

        return Convert.ToBase64String(array.Array ?? [], array.Offset, array.Count);
    }
}
