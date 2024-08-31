namespace ActivityPaint.Core.Extensions;

public static class CharExtensions
{
    public static char ToLower(this char value)
    {
        if (value >= 'A' && value <= 'Z')
        {
            return (char)('a' + (value - 'A'));
        }

        return value;
    }

    public static char ToUpper(this char value)
    {
        if (value >= 'a' && value <= 'z')
        {
            return (char)('A' + (value - 'a'));
        }

        return value;
    }
}
