using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadBL3 : TransferingHeadBL 
    {
        public static int OnRoadGenId = CustomSettings.GetInt("OnRoadGenId").Value;
        public static string OnRoadStdocIdTo = CustomSettings.GetString("OnRoadStdocIdTo");
        public static string OnRoadStdocIdFrom = CustomSettings.GetString("OnRoadStdocIdFrom");
        public static string OnRoadWarehouse = CustomSettings.GetString("OnRoadWarehouse");
        
        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            var sth = (StHead)objects.Default;

            if (e is OlcStHead)
            {
                var csh = (OlcStHead)e;
                if (csh.State == eProjectWeb.Framework.Data.DataRowState.Added)
                    csh.Stid = sth.Stid;
            }
            return b;
        }

        public override void Validate(eProjectWeb.Framework.BL.BLObjectMap objects)
        {
            base.Validate(objects);

            var csh = objects.Get<OlcStHead>();
            if (csh != null)
                eProjectWeb.Framework.RuleServer.Validate(objects, typeof(OlcStHeadRules));
        }
    }
}
