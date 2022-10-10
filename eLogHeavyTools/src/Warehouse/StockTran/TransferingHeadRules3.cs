using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadRules3 : TransferingHeadRules
    {
        public TransferingHeadRules3()
        {
            AddCustomRule(OnRoadRule);
        }

        private void OnRoadRule(RuleValidateContext ctx, StHead value)
        {
            if (value.Fromwhid == TransferingHeadBL3.OnRoadWarehouse && value.Gen != TransferingHeadBL3.OnRoadGenId)
            {
                throw new MessageException("$cannotuseonroadwhidfrom");
            }

            if (value.Stdocid == TransferingHeadBL3.OnRoadStdocIdFrom && value.Gen!= TransferingHeadBL3.OnRoadGenId)
            {
                throw new MessageException("$cannotuseonroadstdocidto");
            }
            /*if (value.Stdocid == TransferingHeadBL3.OnRoadStdocIdTo && value.Towhid != TransferingHeadBL3.OnRoadWarehouse)
            {
                throw new MessageException("$towhidnnotonroad");
            }
            if (value.Stdocid == TransferingHeadBL3.OnRoadStdocIdFrom && value.Fromwhid != TransferingHeadBL3.OnRoadWarehouse)
            {
                throw new MessageException("$fromwhidnnotonroad");
            }*/
        }
    }
}
