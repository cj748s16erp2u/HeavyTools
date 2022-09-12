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

        public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlWhereParameter<T>([DisallowNull] T obj, [DisallowNull] Expression<Func<T, object>> expression, string? paramPrefix = null, string? tablePrefix = "")
        {
            var parameter = expression.Parameters[0];

            var memberNames = VisitExpression(expression);
            return ToSqlWhereParameter(obj, memberNames, paramPrefix, tablePrefix, parameter);
        }

        public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlWhereParameter<T>([DisallowNull] T obj, string? paramPrefix = null, string? tablePrefix = "")
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "t");

            var memberNames = type.GetProperties().Select(p => p.Name).ToList();
            return ToSqlWhereParameter(obj, memberNames, paramPrefix, tablePrefix, parameter);
        }

        private static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlWhereParameter<T>(T obj, IEnumerable<string> memberNames, string? paramPrefix, string? tablePrefix, ParameterExpression parameter)
        {
            var bldr = new StringBuilder();
            var sep = string.Empty;
            if (!string.IsNullOrWhiteSpace(paramPrefix))
            {
                paramPrefix = $"{paramPrefix}_";
            }

            if (tablePrefix == null)
            {
                tablePrefix = parameter.Name;
            }

            var i = 0;
            var parameters = new Dictionary<string, object>();
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
                if (!string.IsNullOrWhiteSpace(tablePrefix))
                {
                    name = $"[{tablePrefix}].{name}";
                }

                string sql;
                if (value is null)
                {
                    sql = CreateSql(name, value!);
                }
                else
                {
                    var parameterName = $"@__{paramPrefix}{memberName}_{i++}";
                    sql = CreateSqlParameter(name, parameterName, value);
                    parameters.Add(parameterName, value);
                }

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    bldr.Append(sep);
                    bldr.Append(sql);
                    sep = " and ";
                }
            }

            return (bldr.ToString(), parameters);
        }

        public static (string fields, string values, IReadOnlyDictionary<string, object> parameters) ToSqlInsertParameter<T>([DisallowNull] T obj, [DisallowNull] Expression<Func<T, object>> expression, string? paramPrefix = null)
        {
            var parameter = expression.Parameters[0];

            var memberNames = VisitExpression(expression);
            return ToSqlInsertParameter(obj, memberNames, paramPrefix, parameter);
        }

        public static (string fields, string values, IReadOnlyDictionary<string, object> parameters) ToSqlInsertParameter<T>([DisallowNull] T obj, string? paramPrefix = null, string? prefix = null)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, prefix ?? "t");

            var memberNames = type.GetProperties().Select(p => p.Name).ToList();
            return ToSqlInsertParameter(obj, memberNames, paramPrefix, parameter);
        }

        private static (string fields, string values, IReadOnlyDictionary<string, object> parameters) ToSqlInsertParameter<T>(T obj, IEnumerable<string> memberNames, string? paramPrefix, ParameterExpression parameter)
        {
            var fieldBldr = new StringBuilder();
            var insertBldr = new StringBuilder();
            var sep = string.Empty;
            if (!string.IsNullOrWhiteSpace(paramPrefix))
            {
                paramPrefix = $"{paramPrefix}_";
            }

            var i = 0;
            var parameters = new Dictionary<string, object>();
            foreach (var memberName in memberNames)
            {
                var expr = CreateExpression<T>(parameter, memberName);
                if (expr is null)
                {
                    continue;
                }

                var value = GetValue(obj, expr);

                string sql;
                if (value is null)
                {
                    sql = $"{value!}";
                }
                else
                {
                    var parameterName = $"@__{paramPrefix}{memberName}_{i++}";
                    sql = parameterName;
                    parameters.Add(parameterName, value);
                }

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    fieldBldr.Append(sep);
                    fieldBldr.Append($"[{memberName}]");

                    insertBldr.Append(sep);
                    insertBldr.Append(sql);
                    sep = ", ";
                }
            }

            return (fieldBldr.ToString(), insertBldr.ToString(), parameters);
        }

        public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlUpdateParameter<T>([DisallowNull] T obj, [DisallowNull] Expression<Func<T, object>> expression, string? tablePrefix = null)
        {
            var parameter = expression.Parameters[0];

            var memberNames = VisitExpression(expression);
            return ToSqlUpdateParameter(obj, memberNames, tablePrefix, parameter);
        }

        public static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlUpdateParameter<T>([DisallowNull] T obj, string? tablePrefix = null)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "t");

            var memberNames = type.GetProperties().Select(p => p.Name).ToList();
            return ToSqlUpdateParameter(obj, memberNames, tablePrefix, parameter);
        }

        private static (string sql, IReadOnlyDictionary<string, object> parameters) ToSqlUpdateParameter<T>(T obj, IEnumerable<string> memberNames, string? paramPrefix, ParameterExpression parameter)
        {
            var bldr = new StringBuilder();
            var sep = string.Empty;
            if (!string.IsNullOrWhiteSpace(paramPrefix))
            {
                paramPrefix = $"{paramPrefix}_";
            }

            var i = 0;
            var parameters = new Dictionary<string, object>();
            foreach (var memberName in memberNames)
            {
                var name = memberName;
                var expr = CreateExpression<T>(parameter, name);
                if (expr is null)
                {
                    continue;
                }

                var value = GetValue(obj, expr);

                name = $"  [{name.ToLowerInvariant()}]";

                string sql;
                if (value is null)
                {
                    sql = CreateSql(name, value!);
                }
                else
                {
                    var parameterName = $"@__{paramPrefix}{memberName}_{i++}";
                    sql = CreateSqlParameter(name, parameterName, value);
                    parameters.Add(parameterName, value);
                }

                if (!string.IsNullOrWhiteSpace(sql))
                {
                    bldr.Append(sep);
                    bldr.Append(sql);
                    sep = $",{Environment.NewLine}";
                }
            }

            return (bldr.ToString(), parameters);
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

        private static string CreateSqlParameter(string memberName, string parameterName, object value)
        {
            if (value is null)
            {
                var sValue = ConvertToString(value);
                return $"{memberName} is {sValue}";
            }

            return $"{memberName} = {parameterName}";
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
