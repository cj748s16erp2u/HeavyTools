using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Common
{
    public class ActionRulesMandotary
    {
        private List<ActionRulesMandotaryItem> Items = new List<ActionRulesMandotaryItem>();
        public void Validate(eProjectWeb.Framework.Rules.RuleValidateContext ctx, Entity entity)
        {
            foreach (var item in Items)
            {
                if (entity[item.Field].Equals(item.Value)) {
                    foreach (var mf in item.MandatoryFields)
                    {
                        if (entity[mf] == null)
                        {
                            ctx.AddErrorField(mf, Translator.Translate("$validation_error_mandatory", Translator.Translate("$"+mf.Name)));
                        }
                    }
                }
            }
        }

        internal void Add(Field field, object value, Field[] mandatoryFields)
        {
            Items.Add(new ActionRulesMandotaryItem() { Field = field, Value = value, MandatoryFields = mandatoryFields });
        }
    }
    public class ActionRulesMandotaryItem
    {
        public Field Field { get; set; }
        public object Value { get; set; }
        public Field[] MandatoryFields { get; set; }
    }
}
