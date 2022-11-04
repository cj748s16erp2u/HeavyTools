using eLog.Base.Common;
using eLog.Base.Setup.StDoc;
using eLog.Base.Warehouse.Common;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public abstract class OlcWhZTranHeadRules : TypedBaseRuleSet<OlcWhZTranHead>
    {
        protected StDocType StDocType;
        protected const string STDOC = "STDOC";

        public OlcWhZTranHeadRules()
            : this(StDocType.Unknown)
        {
        }

        public OlcWhZTranHeadRules(StDocType stDocType)
            : base(true)
        { 
            StDocType = stDocType;
        }

        protected static List<string> m_closedFieldsModifyable;
        public static List<string> ClosedFieldsModifyable
        {
            get { return m_closedFieldsModifyable; }
        }

        private void StIdRule(RuleValidateContext ctx, OlcWhZTranHead olcWhZTranHead)
        {
            NewAllowedValuesRule(ctx, olcWhZTranHead, OlcWhZTranHead.FieldStid, "$rule_stidreqnull", null);
            NotEditableEntityRule(ctx, olcWhZTranHead, OlcWhZTranHead.FieldStid, "$rule_stidnoteditable");
        }

        private void CmpIdRule(RuleValidateContext ctx, OlcWhZTranHead olcWhZTranHead)
        {
            if (!olcWhZTranHead.Cmpid.HasValue)
                return;

            CompanyRuleStd(ctx, olcWhZTranHead, StHead.FieldCmpid, "$rule_cmpidnoteditable");
        }
        private void GenRule(RuleValidateContext ctx, OlcWhZTranHead olcWhZTranHead)
        {
            NotEditableEntityRule(ctx, olcWhZTranHead, StHead.FieldGen, "$rule_gennoteditable");
            if (ctx.ActionID == eProjectWeb.Framework.BL.ActionID.New)
            {
                if (olcWhZTranHead.Gen.GetValueOrDefault() < 0)
                    ctx.AddErrorField(StHead.FieldGen.Name, "$rule_gennegative");
            }
        }
        protected void WarehouseRule(RuleValidateContext ctx, OlcWhZTranHead olcWhZTranHead, Dictionary<SetupRecordExistsRightsResult, string> msgList, string msgLineExistsWhNotEditable, eProjectWeb.Framework.Data.Field fieldWarehouseId)
        {
            if (NullAndMandatory(olcWhZTranHead, fieldWarehouseId))
                // kotelezo megadni az erteket, de nincs megadva. Mashol fog hibat okozni
                return;

            AdditionalRules.WarehouseExists(olcWhZTranHead, fieldWarehouseId, OlcWhZTranHead.FieldCmpid, eLog.Base.Maintenance.UserWh.UserWhUWRType.STUpdate, ctx,
                msgList, this.GetType().Namespace);
            // Csak akkor modosithato, ha nincs tetele
            if (olcWhZTranHead.State == DataRowState.Modified)
            {
                if (WarehouseSqlFunctions.StockTranLineExistsStrict(olcWhZTranHead.Stid.GetValueOrDefault()))
                    NotEditableEntityRule(ctx, olcWhZTranHead, fieldWarehouseId, msgLineExistsWhNotEditable);
            }
        }
        protected bool NullAndMandatory(Entity entity, Field field)
        {
            if (this.ERules[field.Name].Mandatory)
            {
                object o = entity[field];
                if (o is StringN && StringN.IsNullOrEmpty((StringN)o))
                    o = null;

                if (o == null)
                    return true;
            }
            return false;
        }
    }
}
