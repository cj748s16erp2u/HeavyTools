using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.Base.Purchase.Pinv;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvAssignOtherEditTab3 : CodaInt.Base.Purchase.Pinv.PinvAssignOtherEditTab2
    {
        protected override CostLine DefaultPageLoad(PageUpdateArgs args)
        {
            CostLine e = base.DefaultPageLoad(args);

            if (e == null)
            {
                return e;
            }

            if (args.ActionID == ActionID.New)
            {
                this.ctrlCosttypeid.SetValue(this.GetDefaultCostTypeId(), args: args);
            }

            return e;
        }

        protected override void DefaultPageLoadEx(DefaultPageLoadExContext context)
        {
            base.DefaultPageLoadEx(context);
            if (context.Costline == null)
                return;

            OlcCostLine olcCostLine = null;
            if (context.Costline.Costlineid.HasValue && (context.PUArgs.ActionID == eProjectWeb.Framework.BL.ActionID.Modify || context.PUArgs.ActionID == eProjectWeb.Framework.BL.ActionID.View))
            {
                olcCostLine = OlcCostLine.Load(context.Costline.Costlineid);
            }

            if (olcCostLine == null)
            {
                olcCostLine = OlcCostLine.CreateNew();
            }
            OlcCostLineDataBind(olcCostLine, false);
        }

        protected override eProjectWeb.Framework.BL.BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, Base.Purchase.Pinv.CostLine costLine)
        {
            var map = base.SaveControlsToBLObjectMap(args, costLine);

            OlcCostLine olcCostLine = null;
            if (costLine.Costlineid.HasValue)
                olcCostLine = OlcCostLine.Load(costLine.Costlineid);
            if (olcCostLine == null)
            {
                olcCostLine = OlcCostLine.CreateNew();
            }
            OlcCostLineDataBind(olcCostLine, true);
            if (olcCostLine.Costlineid == null)
                olcCostLine.Costlineid = costLine.Costlineid;

            map.Add(olcCostLine);

            return map;
        }

        // az olc_costline mezok is az EditGroup1 panelen vannak, mint az ols_costline mezok
        // csak azokat a mezoket toltjuk az olc_costline alapjan, melyek nincsenek benne az ols_costline-ban
        protected void OlcCostLineDataBind(INamedObjectCollection o, bool save)
        {
            var s1 = Base.Purchase.Pinv.CostLine.GetSchema().Fields;
            var s2 = OlcCostLine.GetSchema().Fields;
            var fields = s2.Where(sc => !s1.Any(sb => string.Equals(sb.Name, sc.Name, StringComparison.OrdinalIgnoreCase))).ToList(); // azok a mezoke, mely az olc_costline-ban benne vannak, de az ols_costline-ban nem
            foreach (var c in EditGroup1.ControlArray)
            {
                if (string.IsNullOrEmpty(c.DataField))
                    continue;

                var field = fields.FirstOrDefault(f => string.Equals(f.Name, c.DataField, StringComparison.OrdinalIgnoreCase));
                if (field == null)
                    continue;

                c.DataBind(o, save);
            }
        }

        protected string GetDefaultCostTypeId()
        {
            return CustomSettings.GetString("PINVASSIGNOTHERDefaultCostType");
        }


    }
}
