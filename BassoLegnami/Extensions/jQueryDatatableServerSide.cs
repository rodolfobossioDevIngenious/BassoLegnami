﻿using System.Linq;
using System;
using System.Linq.Expressions;

namespace BassoLegnami.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, bool ascendingDirection)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");

            Expression body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            IOrderedQueryable<T> queryable = ascendingDirection ?
                (IOrderedQueryable<T>)Queryable.OrderBy(query.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<T>)Queryable.OrderByDescending(query.AsQueryable(), (dynamic)Expression.Lambda(body, param));

            return queryable;
        }

        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> sourceList, string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                return sourceList;
            }

            try
            {

                System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> properties = typeof(T).GetProperties()
                    .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);

                //Expression
                sourceList = sourceList.Where(c =>
                    properties.Any(p => p.GetValue(c) != null && p.GetValue(c).ToString()
                        .Contains(query, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sourceList;
        }
    }
}