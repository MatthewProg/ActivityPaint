using System.Reflection;
using System.Runtime.CompilerServices;

namespace ActivityPaint.Core.Extensions;

public static class TypeExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Necessary to obtain method name.")]
    public static MethodInfo? GetMethodOfGeneric(this Type type, Delegate method, [CallerArgumentExpression(nameof(method))] string methodName = null!)
    {
        var lastDot = methodName.LastIndexOf('.') + 1;
        var onlyMethodName = methodName[lastDot..];

        return type.GetMethod(onlyMethodName);
    }
}
