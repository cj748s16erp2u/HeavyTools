using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using FluentValidation.Results;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;

[System.Diagnostics.DebuggerStepThrough]
public static class LogicServiceBaseExtensions
{
    /// <summary>
    /// Throws a validation exception of type <see cref="LogicServiceValidationException{TEntity, TService}"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <param name="service">The service which throws the exception.</param>
    /// <param name="failures">List of the <see cref="ValidationFailure"/></param>
    /// <param name="entity">The validated entity.</param>
    /// <param name="originalEntity">The optional original entity.</param>
    public static void ThrowException<TEntity>(this ILogicService<TEntity> service, IEnumerable<ValidationFailure> failures, TEntity entity, TEntity? originalEntity = null)
        where TEntity : class, IEntity
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        if (failures is null)
        {
            throw new ArgumentNullException(nameof(failures));
        }

        var fieldName = failures.Count() == 1 ? failures.First().PropertyName : null;

        var genType = CreateGenericValidationExceptionType(service);
        var ctor = genType.GetConstructor(new[] { typeof(IEnumerable<ValidationFailure>), typeof(TEntity), typeof(TEntity), typeof(string) });
        if (ctor is not null)
        {
            var instance = (Exception)ctor.Invoke(new object?[] { failures, entity, originalEntity, fieldName });

            throw instance;
        }

#pragma warning disable S112 // General exceptions should never be thrown
        throw new Exception(string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage)));
#pragma warning restore S112 // General exceptions should never be thrown
    }

    /// <summary>
    /// Throws a validation exception of type <see cref="LogicServiceValidationException{TEntity, TService}"/>
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity.</typeparam>
    /// <param name="service">The service which throws the exception.</param>
    /// <param name="message">The message to be thrown.</param>
    /// <param name="entity">The validated entity.</param>
    /// <param name="fieldName">The name of the affected field</param>
    public static void ThrowException<TEntity>(this ILogicService<TEntity> service, string message, TEntity? entity = null, string? fieldName = null)
        where TEntity : class, IEntity
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var genType = CreateGenericExceptionType(service);
        var ctor = genType.GetConstructor(new[] { typeof(string), typeof(TEntity), typeof(string) });
        if (ctor is not null)
        {
            var instance = (Exception)ctor.Invoke(new object?[] { message, entity, fieldName });

            throw instance;
        }

#pragma warning disable S112 // General exceptions should never be thrown
        throw new Exception(message);
#pragma warning restore S112 // General exceptions should never be thrown
    }

    private static Type CreateGenericExceptionType<TEntity>(ILogicService<TEntity> service)
        where TEntity : class, IEntity
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var serviceType = service.GetType();
        var exceptionType = typeof(LogicServiceException<,>);
        var genType = exceptionType.MakeGenericType(new[] { typeof(TEntity), serviceType });
        return genType;
    }

    private static Type CreateGenericValidationExceptionType<TEntity>(ILogicService<TEntity> service)
        where TEntity : class, IEntity
    {
        if (service is null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        var serviceType = service.GetType();
        var exceptionType = typeof(LogicServiceValidationException<,>);
        var genType = exceptionType.MakeGenericType(new[] { typeof(TEntity), serviceType });
        return genType;
    }
}
