using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidation;

public static class ValidationContextExtension
{
    public static void AddFailure<TEntity>(this ValidationContext<TEntity> context, Expression<Func<TEntity, object>> fieldExpression, string message)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (fieldExpression is null)
        {
            throw new ArgumentNullException(nameof(fieldExpression));
        }

        var fieldName = GetFieldName(fieldExpression);
        context.AddFailure(new FluentValidation.Results.ValidationFailure(fieldName, message));
    }

    private static string GetFieldName<TEntity>(Expression<Func<TEntity, object>> expr)
    {
        if (expr is null)
        {
            throw new ArgumentNullException(nameof(expr));
        }

        //expr.Body

        return string.Empty;
    }
}
