using System.Text;

namespace ActivityPaint.Application.BusinessLogic.Tests.Mock;

public static class TestExtensions
{
    public static byte[] GetBytes(this Encoding encoding, string s, bool addPreamble)
    {
        return addPreamble
            ? [..encoding.GetPreamble(), ..encoding.GetBytes(s)]
            : encoding.GetBytes(s);
    }

    public static byte[] ReadBytes(this Stream stream)
    {
        using var memoryStream = new MemoryStream();

        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream.ToArray();
    }
}
