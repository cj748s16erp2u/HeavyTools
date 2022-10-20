using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;

namespace FluentValidation;

[System.Diagnostics.DebuggerStepThrough]
internal class ReadOnlyValidator<T, TProperty> : PropertyValidator<T, TProperty>, IReadOnlyValidator
    where T : class, eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces.IEntity
{
    private Expression<Func<T, TProperty>> expression;

    public ReadOnlyValidator(Expression<Func<T, TProperty>> expression)
    {
        this.expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }

    public override string Name => "ReadOnlyValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty newValue)
    {
        var origHead = context.GetOriginalEntity();
        if (origHead is null)
        {
            return false;
        }    

        var func = this.expression.Compile();
        var origValue = func(origHead);
        return object.Equals(origValue, newValue);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return Localized(errorCode, Name);
    }
}

public interface IReadOnlyValidator : IPropertyValidator
{
}