using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable enable

namespace Anshan.Core
{
    /// <summary>
    ///     A utility class provides the functionality to build an object using its properties
    /// </summary>
    /// <typeparam name="T"> The type of object </typeparam>
    /// <typeparam name="TResult"> The type of object that is built </typeparam>
    public abstract class Builder<T, TResult> where T : class, new()
    {
        protected readonly T Instance = new();

        protected void SetValue<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
            where TProperty : notnull
        {
            var memberSelectorExpression = (MemberExpression) expression.Body;

            var property = (PropertyInfo) memberSelectorExpression.Member;

            property.SetValue(Instance, value, null);
        }

        public abstract TResult Build();

        public static implicit operator TResult(Builder<T, TResult> builder)
        {
            return builder.Build();
        }
    }

    /// <summary>
    ///     A utility class provides the functionality to build an object using its properties
    /// </summary>
    /// <typeparam name="T"> The type of object </typeparam>
    public abstract class Builder<T> : Builder<T, T> where T : class, new()
    {
        public override T Build()
        {
            return Instance;
        }
    }

    public class GenericBuilder<T> : Builder<T> where T : class, new()
    {
        public GenericBuilder<T> With<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
            where TProperty : notnull
        {
            SetValue(expression, value);

            return this;
        }
    }
}