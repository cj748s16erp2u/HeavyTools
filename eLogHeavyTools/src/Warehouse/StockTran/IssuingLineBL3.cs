using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class IssuingLineBL3 : IssuingLineBL
    {
        protected override void PreDelete(Key k)
        {
            base.PreDelete(k);
            var st = StLine.Load(k);
            if (st != null)
            {
                //Visszafoglaljuk a vevői rendelést
                if (st.Sordlineid.HasValue)
                {
                    var so = SordLine.Load(st.Sordlineid);
                    if (so.Resid.HasValue)
                    {
                        var stqty = ConvertUtils.ToDecimal(SqlDataAdapter.ExecuteSingleValue(DB.Main,
                            $"select sum(ordqty) from ols_stline where stlineid<>{st.Stlineid} and sordlineid={so.Sordlineid}")).GetValueOrDefault(0);
                         
                        var res = Reserve.Load(so.Resid);
                        res.Resqty = so.Ordqty - stqty;
                        var resBL = ReserveBL.New();
                        var map = resBL.CreateBLObjects();
                        map.Default = res;
                        resBL.Save(map);
                    }
                }
            }
        }
    }
}
