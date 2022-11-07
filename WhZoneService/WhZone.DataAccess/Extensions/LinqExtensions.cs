using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace System.Linq;

public static class LinqExtensions
{
    public static Task<TSource?> FirstOrDefaultWithNoLockAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }))
        {
            return source.FirstOrDefaultAsync(cancellationToken);
        }
    }

    public static Task<TSource?> FirstOrDefaultWithNoLockAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadUncommitted
        }))
        {
            return source.FirstOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
