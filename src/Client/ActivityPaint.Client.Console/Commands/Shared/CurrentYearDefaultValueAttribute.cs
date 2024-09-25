using System.ComponentModel;

namespace ActivityPaint.Client.Console.Commands.Shared;

[AttributeUsage(AttributeTargets.Property)]
public class CurrentYearDefaultValueAttribute() : DefaultValueAttribute($"{DateTime.UtcNow.Year}-01-01")
{
}
