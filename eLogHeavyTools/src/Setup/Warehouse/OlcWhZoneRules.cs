using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneRules : eLog.Base.Common.TypedBaseRuleSet<OlcWhZone>
    {
        public OlcWhZoneRules() : base(true, false)
        {
            this.AddCustomRule(this.CheckIsBackground);
            this.AddCustomRule(this.CheckIsPuffer);
            this.AddCustomRule(this.CheckLocDefValues);
        }

        private void CheckIsBackground(RuleValidateContext ctx, OlcWhZone value)
        {
            if (value.Isbackground.GetValueOrDefault() != 0 && (value.State == DataRowState.Added ||
                !Utils.Equals(value[OlcWhZone.FieldIsbackground, DataRowVersion.Original], value[OlcWhZone.FieldIsbackground, DataRowVersion.Current])))
            {
                var bl = OlcWhZoneBL.New();
                if (!bl.CheckZoneIsBackgroundUniqueInWh(value.Whid, value.Whzoneid))
                {
                    ctx.AddErrorField(OlcWhZone.FieldIsbackground, "$validation_error_unique_whzone", $"${OlcWhZone.FieldIsbackground.Name}".eLogTransl());
                }
            }
        }

        private void CheckIsPuffer(RuleValidateContext ctx, OlcWhZone value)
        {
            if (value.Ispuffer.GetValueOrDefault() != 0 && (value.State == DataRowState.Added ||
                !Utils.Equals(value[OlcWhZone.FieldIspuffer, DataRowVersion.Original], value[OlcWhZone.FieldIspuffer, DataRowVersion.Current])))
            {
                var bl = OlcWhZoneBL.New();
                if (!bl.CheckZoneIsPufferUniqueInWh(value.Whid, value.Whzoneid))
                {
                    ctx.AddErrorField(OlcWhZone.FieldIspuffer, "$validation_error_unique_whzone", $"${OlcWhZone.FieldIspuffer.Name}".eLogTransl());
                }
            }
        }

        private void CheckLocDefValues(RuleValidateContext ctx, OlcWhZone value)
        {
            if (value.State != DataRowState.Added)
            {
                OlcWhZoneBL bl = null;
                this.CheckLocDefFieldValue(ctx, value, OlcWhZone.FieldLocdefvolume, OlcWhLocation.FieldVolume, ref bl);
                this.CheckLocDefFieldValue(ctx, value, OlcWhZone.FieldLocdefoverfillthreshold, OlcWhLocation.FieldOverfillthreshold, ref bl);
                this.CheckLocDefFieldValue(ctx, value, OlcWhZone.FieldLocdefismulti, OlcWhLocation.FieldIsmulti, ref bl);
            }
        }

        private void CheckLocDefFieldValue(RuleValidateContext ctx, OlcWhZone value, Field field, Field targetField, ref OlcWhZoneBL bl)
        {
            if (value[field, DataRowVersion.Original] != null &&
                value[field, DataRowVersion.Current] == null)
            {
                bl = bl ?? OlcWhZoneBL.New();
                if (bl.CheckHasLocationWOSpecValue(value.Whzoneid, targetField, OlcWhLocation_LocType.Normal))
                {
                    this.CheckMandatory(ctx, value, field);
                }
            }
        }
    }
}
