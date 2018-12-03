
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SkyMallCore.Core
{
    public static partial class ExtLinq
    {
        //public static IOrderedQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate)
        //    where TEntity : class, new()
        //{
        //    return InvokeSortBy(query, sortPredicate, SortOrder.Ascending);
        //}

        //public static IOrderedQueryable<TEntity> SortByDescending<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate)
        //    where TEntity : class, new()
        //{
        //    return InvokeSortBy(query, sortPredicate, SortOrder.Descending);
        //}

        //https://www.codeproject.com/tips/817372/building-orderby-lambda-expression-from-property-n
        //makes expression for specific prop
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property
            (param, propertyName), typeof(object));   //important to use the Expression.Convert
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        //makes deleget for specific prop
        public static Func<TSource, object> GetFunc<TSource>(string propertyName)
        {
            return GetExpression<TSource>(propertyName).Compile();  //only need compiled expression
        }

        //OrderBy overload
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderBy(GetFunc<TSource>(propertyName));
        }

        //OrderBy overload
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return source.OrderBy(GetExpression<TSource>(propertyName));
        }

        /// <summary>
        /// 字段排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderField<TSource>(this IQueryable<TSource> source, string propertyName, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return OrderBy(source, propertyName);
            }
            return source.OrderByDescending(GetExpression<TSource>(propertyName));
        }

        /// <summary>
        /// 字段排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> OrderField<TSource>(this IQueryable<TSource> source, string propertyName, string sortOrder)
        {
            var sort = SortOrder.Ascending;
            if (sortOrder == "desc")
            {
                sort = SortOrder.Descending;
            }
            return OrderField(source, propertyName, sort);
        }


        private static IOrderedQueryable<TEntity> InvokeSortBy<TEntity>(IQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder)
            where TEntity : class, new()
        {
            var param = sortPredicate.Parameters[0];
            string propertyName = null;
            Type propertyType = null;
            Expression bodyExpression = null;
            if (sortPredicate.Body is UnaryExpression)
            {
                var unaryExpression = sortPredicate.Body as UnaryExpression;
                bodyExpression = unaryExpression.Operand;
            }
            else if (sortPredicate.Body is MemberExpression)
            {
                bodyExpression = sortPredicate.Body;
            }
            else
                throw new ArgumentException(@"The body of the sort predicate expression should be 
                either UnaryExpression or MemberExpression.", "sortPredicate");
            var memberExpression = (MemberExpression)bodyExpression;
            propertyName = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo != null) propertyType = propertyInfo.PropertyType;
            }
            else
                throw new InvalidOperationException(@"Cannot evaluate the type of property since the member expression 
                represented by the sort predicate expression does not contain a PropertyInfo object.");

            var funcType = typeof(Func<,>).MakeGenericType(typeof(TEntity), propertyType);
            var convertedExpression = Expression.Lambda(funcType,
                Expression.Convert(Expression.Property(param, propertyName), propertyType), param);

            var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var sortingMethodName = GetSortingMethodName(sortOrder);
            var sortingMethod = sortingMethods.First(sm => sm.Name == sortingMethodName &&
                                                           sm.GetParameters().Length == 2);
            return (IOrderedQueryable<TEntity>)sortingMethod
                .MakeGenericMethod(typeof(TEntity), propertyType)
                .Invoke(null, new object[] { query, convertedExpression });
        }

        private static string GetSortingMethodName(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "OrderBy";
                case SortOrder.Descending:
                    return "OrderByDescending";
                default:
                    throw new ArgumentException("Sort Order must be specified as either Ascending or Descending.",
            "sortOrder");
            }
        }
    }
}
