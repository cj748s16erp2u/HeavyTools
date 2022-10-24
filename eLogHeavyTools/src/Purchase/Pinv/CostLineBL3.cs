using CodaInt.Base.Bookkeeping.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class CostLineBL3 : CodaInt.Base.Purchase.Pinv.CostLineBL2
    {
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var b = base.PreSave(objects, e);
            if (b)
            {
                var costLine = objects.Default as Base.Purchase.Pinv.CostLine;

                var defaultCostType = CustomSettings.GetString("PinvDefaultCostType");
                if (!string.IsNullOrEmpty(defaultCostType))
                {
                    if (e is Base.Purchase.Pinv.CostLine)
                    {
                        if (eProjectWeb.Framework.ConvertUtils.ToInt32(costLine?.Pinvlineid).HasValue)
                        {
                            if (string.IsNullOrEmpty(costLine.Costtypeid))
                                costLine.Costtypeid = defaultCostType.ToString();
                        }
                    }
                }
            }

            return b;
        }

    }
}
