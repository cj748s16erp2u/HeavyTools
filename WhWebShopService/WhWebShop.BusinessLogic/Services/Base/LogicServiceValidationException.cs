using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using FluentValidation.Results;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;

/// <summary>
/// Validation exception for <see cref="LogicServiceBase{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity">Type of the entity.</typeparam>
/// <typeparam name="TService">Type of the logic service.</typeparam>
/// <seealso cref="LogicServiceException{TEntity, TLogicService}"/>
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
public sealed class LogicServiceValidationException<TEntity, TService> : LogicServiceException<TEntity, TService>
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    where TEntity : class, IEntity
    where TService : class, ILogicService<TEntity>
{
    /// <summary>
    /// Gets the original entity.
    /// </summary>
    public TEntity? OriginalEntity { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceBaseValidationException{TEntity}"/> class.
    /// </summary>
    /// <param name="failures">List of the <see cref="ValidationFailure"/></param>
    /// <param name="entity">The validated entity.</param>
    /// <param name="originalEntity">The optional original entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceValidationException(IEnumerable<ValidationFailure> failures, TEntity? entity, TEntity? originalEntity = null, string? fieldName = null)
        : this(string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage).ToArray()), entity, originalEntity, fieldName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceBaseValidationException{TEntity}"/> class.
    /// </summary>
    public LogicServiceValidationException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceBaseValidationException{TEntity}"/> class.
    /// </summary>
    /// <param name="entity">The validated entity.</param>
    /// <param name="originalEntity">The optional original entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceValidationException(TEntity? entity, TEntity? originalEntity, string? fieldName = null) : base(entity, fieldName)
    {
        this.OriginalEntity = originalEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceBaseValidationException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="entity">The validated entity.</param>
    /// <param name="originalEntity">The optional original entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceValidationException(string? message, TEntity? entity, TEntity? originalEntity = null, string? fieldName = null) : base(message, entity, fieldName)
    {
        this.OriginalEntity = originalEntity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceBaseValidationException{TEntity}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference
    /// (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    /// <param name="entity">The validated entity.</param>
    /// <param name="originalEntity">The optional original entity.</param>
    public LogicServiceValidationException(string? message, Exception? innerException, TEntity? entity, TEntity? originalEntity = null, string? fieldName = null)
        : base(message, innerException, entity, fieldName)
    {
        this.OriginalEntity = originalEntity;
    }
}
