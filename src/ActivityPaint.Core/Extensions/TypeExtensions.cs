using System.Reflection;
using System.Runtime.CompilerServices;

namespace ActivityPaint.Core.Extensions;

public static class TypeExtensions
{
    public static MethodInfo? GetMethodOfGeneric(this Type type, Delegate method, [CallerArgumentExpression(nameof(method))] string methodName = null!)
    {
        var lastDot = methodName.LastIndexOf('.') + 1;
        var onlyMethodName = methodName[lastDot..];

        return type.GetMethod(onlyMethodName);
    }
}
