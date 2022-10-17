using eLog.Base.Warehouse.StockTran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingLineBL3 : TransferingLineBL
    {
        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            var sth = (StLine)objects.Default;

            var csh = e as OlcStLine;
            if (csh != null)
            {
                if (csh.State == eProjectWeb.Framework.Data.DataRowState.Added)
                    csh.Stlineid = sth.Stlineid;
            }
            return b;
        }
    }
}
