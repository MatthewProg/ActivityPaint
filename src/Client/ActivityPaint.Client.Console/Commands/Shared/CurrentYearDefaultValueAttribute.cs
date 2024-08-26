using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Shared;

[AttributeUsage(AttributeTargets.Property)]
public class CurrentYearDefaultValueAttribute : DefaultValueAttribute
{
    public CurrentYearDefaultValueAttribute() : base($"{DateTime.UtcNow.Year}-01-01") { }
}
