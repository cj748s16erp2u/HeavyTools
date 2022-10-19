using eLog.Base.Masters.Item;
using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sord
{
    internal class SordStBL3 : SordStBL
    {
        protected override void CreateStLine_BeforeSave(CreateStLineArgs args, StLine stLine)
        {
            if (stLine.Movqty2 > stLine.Ordqty2)
            {
                var i = Item.Load(stLine.Itemid);
                var sol = SordLine.Load(stLine.Sordlineid);
                var soh = SordHead.Load(sol.Sordid);


                throw new MessageException("$movqtycannotbiggerordqty", soh.Docnum, i.Itemcode, sol.Ordqty, stLine.Movqty2);
            }
        }
    }
}
