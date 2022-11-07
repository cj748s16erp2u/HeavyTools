using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
/// <summary>
/// Exception for <see cref="LogicServiceBase{TEntity}"/>
/// </summary>
[System.Diagnostics.DebuggerStepThrough]
public class LogicServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException"/> class.
    /// </summary>
    protected LogicServiceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException"/> class.
    /// </summary>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(object? entity, string? fieldName = null)
    {
        this.Object = entity;
        this.FieldName = fieldName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(string? message, object? entity, string? fieldName = null) : base(message)
    {
        this.Object = entity;
        this.FieldName = fieldName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference
    /// (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(string? message, Exception? innerException, object? entity, string? fieldName = null) : base(message, innerException)
    {
        this.Object = entity;
        this.FieldName = fieldName;
    }

    /// <summary>
    /// Gets the affected entity.
    /// </summary>
    public object? Object { get; }

    /// <summary>
    /// Gets the affected field's name.
    /// </summary>
    public string? FieldName { get; }
}

/// <summary>
/// Exception for <see cref="LogicServiceBase{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity">Type of the entity.</typeparam>
/// <typeparam name="TService">Type of the logic service.</typeparam>
#pragma warning disable S2326 // Unused type parameters should be removed
[System.Diagnostics.DebuggerStepThrough]
public class LogicServiceException<TEntity, TService> : LogicServiceException
#pragma warning restore S2326 // Unused type parameters should be removed
    where TEntity : class, IEntity
    where TService : class, ILogicService<TEntity>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException{TEntity, TLogicService}"/> class.
    /// </summary>
    protected LogicServiceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException{TEntity, TLogicService}"/> class.
    /// </summary>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(TEntity? entity, string? fieldName = null) : base(entity, fieldName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException{TEntity, TLogicService}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(string? message, TEntity? entity, string? fieldName = null) : base(message, entity, fieldName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicServiceException{TEntity, TLogicService}"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference
    /// (Nothing in Visual Basic) if no inner exception is specified.
    /// </param>
    /// <param name="entity">The affected entity.</param>
    /// <param name="fieldName">Name of the affected field.</param>
    public LogicServiceException(string? message, Exception? innerException, TEntity? entity, string? fieldName = null) : base(message, innerException, entity, fieldName)
    {
    }

    /// <summary>
    /// Gets the affected entity.
    /// </summary>
    public TEntity? Entity => (TEntity?)this.Object;
}
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
