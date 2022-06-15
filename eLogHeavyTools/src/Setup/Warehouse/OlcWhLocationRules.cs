using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationRules : eLog.Base.Common.TypedBaseRuleSet<OlcWhLocation>
    {
        public OlcWhLocationRules() : base(true, false)
        {
            this.AddCustomRule(this.LocTypeRule);
        }

        protected const string OLCWHZONE = nameof(OlcWhZone);

        protected override void BeforeCustomRules(RuleValidateContext ctx, OlcWhLocation value)
        {
            base.BeforeCustomRules(ctx, value);

            var dict = new Dictionary<string, object>();

            if (value.Whzoneid != null)
            {
                var zone = OlcWhZone.Load(value.Whzoneid);
                if (zone != null)
                {
                    dict[OLCWHZONE] = zone;
                }
            }

            ctx.InternalCustomData = dict;
        }

        protected object GetInternalCustomData(RuleValidateContext ctx, string id)
        {
            if (ctx.InternalCustomData is IDictionary<string, object>)
            {
                var dict = (IDictionary<string, object>)ctx.InternalCustomData;

                object obj;
                if (dict.TryGetValue(id, out obj))
                {
                    return obj;
                }
            }

            return null;
        }

        protected OlcWhZone GetOlcWhZone(RuleValidateContext ctx)
        {
            return this.GetInternalCustomData(ctx, OLCWHZONE) as OlcWhZone;
        }

        private void LocTypeRule(RuleValidateContext ctx, OlcWhLocation value)
        {
            if (value?.Loctype != null)
            {
                switch (value.Loctype)
                {
                    case (int)OlcWhLocation_LocType.Normal:
                        this.ValidateNormalLocation(ctx, value);
                        break;
                    case (int)OlcWhLocation_LocType.Moving:
                        this.ValidateMovingLocation(ctx, value);
                        break;
                    case (int)OlcWhLocation_LocType.TransferArea:
                        this.ValidateTransferAreaLocation(ctx, value);
                        break;
                }
            }
        }

        private void ValidateNormalLocation(RuleValidateContext ctx, OlcWhLocation value)
        {
            this.CheckForbidden(ctx, value, OlcWhLocation.FieldMovloctype);

            if (value.Whzoneid == null)
            {
                this.CheckMandatory(ctx, value, OlcWhLocation.FieldVolume);
                this.CheckMandatory(ctx, value, OlcWhLocation.FieldOverfillthreshold);
                this.CheckMandatory(ctx, value, OlcWhLocation.FieldIsmulti);
            }
            else
            {
                var zone = this.GetOlcWhZone(ctx);
                if (zone.Locdefvolume == null)
                {
                    this.CheckMandatory(ctx, value, OlcWhLocation.FieldVolume);
                }

                if (zone.Locdefoverfillthreshold == null)
                {
                    this.CheckMandatory(ctx, value, OlcWhLocation.FieldOverfillthreshold);
                }

                if (zone.Locdefismulti == null)
                {
                    this.CheckMandatory(ctx, value, OlcWhLocation.FieldIsmulti);
                }
            }

            this.CheckMandatory(ctx, value, OlcWhLocation.FieldCrawlorder);

            this.CheckForbidden(ctx, value, OlcWhLocation.FieldCapacity);
            this.CheckForbidden(ctx, value, OlcWhLocation.FieldCapunitid);
        }

        private void ValidateMovingLocation(RuleValidateContext ctx, OlcWhLocation value)
        {
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldName);
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldMovloctype);
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldOverfillthreshold);
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldCapacity);
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldCapunitid);
        }

        private void ValidateTransferAreaLocation(RuleValidateContext ctx, OlcWhLocation value)
        {
            this.CheckMandatory(ctx, value, OlcWhLocation.FieldName);
        }

        protected void CheckForbidden(RuleValidateContext ctx, Entity entity, Field field, string label = null)
        {
            CheckForbidden(ctx, entity[field], field.Name, label);
        }

        protected void CheckForbidden(RuleValidateContext ctx, object value, string fieldName, string label = null)
        {
            ctx.PushObject(value);
            ctx.PushPathText(fieldName);
            try
            {
                var r = new ForbiddenRule
                {
                    Enabled = true
                };

                if (!string.IsNullOrEmpty(label))
                {
                    r.FieldNameLabelId = label;
                }

                r.Validate(ctx);
            }
            finally
            {
                ctx.PopPathText();
                ctx.PopObject();
            }
        }

        protected class ForbiddenRule : RuleBase
        {
            public string FieldNameLabelId = null; // Ha mezoatnevezesek vannak, akkor hibanal ne a Field.Name-t, hanem ezt irja ki

            public ForbiddenRule()
            {
            }

            public override bool Render(RuleRenderContext ctx)
            {
                if (Enabled)
                {
                    ctx.WriteSep();
                    ctx.w.Write("fbdn:1");
                }

                return Enabled;
            }

            // true ha hibas, false ha jo
            public static bool HasError(object v)
            {
                var error = false;

                if (v is StringN)
                {
                    v = (string)v;
                }

                if (v != null)
                {
                    error = true;
                }
                else if (v is string)
                {
                    var s = (string)v;
                    error = !string.IsNullOrWhiteSpace(s);
                }

                return error;
            }

            public override void Validate(RuleValidateContext ctx)
            {
                if (!Enabled)
                    return;

                object v = ctx.GetObject();
                if (HasError(v))
                {
                    string fieldName = ctx.GetPathTextLastElement();
                    AddError(ctx, fieldName);
                }
            }

            public bool Validate(RuleValidateContext ctx, string fieldName, object value)
            {
                if (HasError(value))
                {
                    AddError(ctx, fieldName);
                    return false;
                }
                return true;
            }

            public bool Validate(RuleValidateContext ctx, Field field, object value)
            {
                return Validate(ctx, field.Name, value);
            }

            public static string GetErrorMsg(string fieldName)
            {
                string fieldName2;
                if (fieldName != null)
                {
                    if (fieldName.StartsWith("$"))
                        fieldName2 = eProjectWeb.Framework.Lang.Translator.Translate(fieldName);
                    else
                    {
                        fieldName2 = eProjectWeb.Framework.Lang.Translator.Translate("$" + fieldName);
                        if (fieldName2 == "$" + fieldName) // nem mezonev volt benne, hanem mar forditas
                            fieldName2 = fieldName;
                    }
                }
                else
                    fieldName2 = fieldName;

                return eProjectWeb.Framework.Lang.Translator.Translate("$validation_error_forbidden", fieldName2);
            }

            public virtual void AddError(RuleValidateContext ctx, string fieldName)
            {
                if (!string.IsNullOrEmpty(this.FieldNameLabelId))
                    fieldName = this.FieldNameLabelId;
                ctx.AddError(new ValidationError(fieldName, RuleFieldErrorType.Mandatory, GetErrorMsg(fieldName)));
            }

            public virtual void AddError(RuleValidateContext ctx, Field field)
            {
                AddError(ctx, field.Name);
            }
        }
    }
}
