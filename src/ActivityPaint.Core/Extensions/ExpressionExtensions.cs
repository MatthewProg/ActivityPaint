using System.Linq.Expressions;
using System.Reflection;

namespace ActivityPaint.Core.Extensions;

public static class ExpressionExtensions
{
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");
        }

        var type = typeof(TSource);
        if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
        {
            throw new ArgumentException($"Expression '{expression}' refers to a property that is not from type {type}.");
        }

        return propInfo;
    }
}
