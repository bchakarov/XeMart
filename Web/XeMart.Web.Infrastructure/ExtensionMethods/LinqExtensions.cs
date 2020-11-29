namespace XeMart.Web.Infrastructure.ExtensionMethods
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class LinqExtensions
    {
        public static IQueryable<T> MyOrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, string.Empty);

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = isAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
