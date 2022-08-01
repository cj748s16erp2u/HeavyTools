using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers
{
    public static class Utils
    {
        public static string ToSql<T>([DisallowNull] T obj, [DisallowNull] Expression<Func<T, object>> expression, string? prefix = null)
        {
            var parameter = expression.Parameters[0];

            var memberNames = VisitExpression(expression);
            return ToSql(obj, memberNames, prefix, parameter);
        }

        public static string ToSql<T>([DisallowNull] T obj, string? prefix = null)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "t");

            var memberNames = type.GetProperties().Select(p => p.Name).ToList();
            return ToSql(obj, memberNames, prefix, parameter);
        }

        public static string ToSqlValue(object? value)
        {
            return ConvertToString(value);
        }

        private static string ToSql<T>(T obj, IEnumerable<string> memberNames, string? prefix, ParameterExpression parameter)
        {
            var bldr = new StringBuilder();
            var sep = string.Empty;

            foreach (var memberName in memberNames)
            {
                var name = memberName;
                var expr = CreateExpression<T>(parameter, name);
                if (expr is null)
                {
                    continue;
                }

                var value = GetValue(obj, expr);

                name = $"[{name.ToLowerInvariant()}]";
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    name = $"[{prefix}].{name}";
                }

                var sql = CreateSql(name, value);

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    bldr.Append(sep);
                    bldr.Append(sql);
                    sep = " and ";
                }
            }

            return bldr.ToString();
        }

        private static Expression<Func<T, object>>? CreateExpression<T>(ParameterExpression parameter, string name)
        {
            var type = typeof(T);
            var prop = type.GetProperty(name);
            if (prop is null)
            {
                return null;
            }

            Expression expr = Expression.Property(parameter, prop);
            expr = Expression.Convert(expr, typeof(object));
            return Expression.Lambda<Func<T, object>>(expr, new[] { parameter });
        }

        private static string CreateSql(string memberName, object value)
        {
            var sValue = ConvertToString(value);
            var op = value is null ? "is" : "=";
            return $"{memberName} {op} {sValue}";
        }

        private static string ConvertToString(object? value)
        {
            if (value is null)
            {
                return $"null";
            }

            var type = value.GetType();
            var typeCode = Type.GetTypeCode(type);

            return typeCode switch
            {
                TypeCode.Char or TypeCode.String => $"'{value}'",
                TypeCode.Boolean => $"{Convert.ToInt16(value)}",
                TypeCode.DateTime => $"'{value:yyyyMMdd HH:mm:ss}'",
                _ => $"{value}",
            };
        }

        private static object GetValue<T>(T obj, Expression<Func<T, object>> predicate)
        {
            var func = predicate.Compile();
            return func.Invoke(obj);
        }

        private static IEnumerable<string> VisitExpression(LambdaExpression expr)
        {
            if (expr.Body is MemberExpression mExpr)
            {
                return new[] { mExpr.Member.Name };
            }
            else if (expr.Body is UnaryExpression uExpr)
            {
                return VisitExpression(uExpr.Operand);
            }
            else if (expr.Body is NewExpression nExpr)
            {
                return VisitExpression(nExpr.Members);
            }
            else
            {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new Exception($"Unhandled type of body: {expr.Body.GetType().FullName}");
#pragma warning restore S112 // General exceptions should never be thrown
            }
        }

        private static IEnumerable<string> VisitExpression(IEnumerable<MemberInfo>? members)
        {
            if (members is null)
            {
                return Enumerable.Empty<string>();
            }

            return members.Select(m => m.Name).ToList();
        }

        private static IEnumerable<string> VisitExpression(Expression expr)
        {
            if (expr is MemberExpression mExpr)
            {
                return new[] { mExpr.Member.Name };
            }
            else
            {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new Exception($"Unhandled type of body: {expr.GetType().FullName}");
#pragma warning restore S112 // General exceptions should never be thrown
            }
        }
    }
}
