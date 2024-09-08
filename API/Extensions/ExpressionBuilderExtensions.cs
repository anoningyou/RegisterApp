using System;
using System.Linq.Expressions;

namespace API.Extensions;

/// <summary>
/// Represents an expression builder.
/// </summary>
public static class ExpressionBuilderExtensions
{
    /// <summary>
    /// Creates a new expression.
    /// </summary>
    public static Expression<Func<T, bool>> New<T>()
    {
        return x => true;
    }

    /// <summary>
    /// Creates a new expression.
    /// </summary>
    public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> expression)
    {
        return expression;
    }

    /// <summary>
    /// Combines two expressions using the AND operator.
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                left.Body,
                Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
    }

    /// <summary>
    /// Combines two expressions using the OR operator.
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                left.Body,
                Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
    }
}
