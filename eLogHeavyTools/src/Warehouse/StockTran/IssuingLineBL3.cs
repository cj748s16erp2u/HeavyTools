using eLog.Base.Manufacture.Common;
using eLog.Base.Project;
using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Common;
using eLog.Base.Warehouse.Reserve;
using eLog.Base.Warehouse.StockTran;
using eLog.HeavyTools.InterfaceCaller;
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
        protected void PreDeleteHeavyTools(Key k, out int? resid, out decimal? resqty)
        {
            resid = null;
            resqty= null;
             
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
                         
                        var res = Base.Warehouse.Reserve.Reserve.Load(so.Resid);

                        resid = res.Resid;
                        resqty = res.Resqty;


                        res.Resqty = so.Ordqty - stqty;
                        var resBL = ReserveBL.New();
                        var map = resBL.CreateBLObjects();
                        map.Default = res;
                        resBL.Save(map);
                    }
                }
            }
        }
        protected void PreDeleteBase(Key k)
        {
            base.PreDelete(k);
            StLine stLine = Entity<StLine>.Load(k);
            if (stLine == null)
            {
                return;
            }

            m_common.PreDeleteLine(k, stLine, this);
            int stockTranCorrType = WarehouseSqlFunctions.GetStockTranCorrType(stLine.Stid.GetValueOrDefault());
            if (stockTranCorrType == 1)
            {
                ReserveStock(stLine.Stlineid.GetValueOrDefault(), -stLine.Dispqty.GetValueOrDefault());
            }

            if (stLine.Reqid.HasValue)
            {
                decimal num = -stLine.Movqty.GetValueOrDefault();
                if (num != 0m)
                {
                    StHead stHead = StHead.Load(stLine.Stid);
                    if (stHead.Sttype.GetValueOrDefault() == 2)
                    {
                        ManufactureSqlFunctions.RequirementIncreaseMovQty(stLine.Reqid.Value, num);
                        WarehouseSqlFunctions.GetHeadBL(StDocType.Issuing).RecalcManufUseStatusByReqId(stLine.Reqid.Value);
                    }
                    else if (stHead.Sttype.GetValueOrDefault() == 3)
                    {
                        ManufactureSqlFunctions.RequirementIncreasePTransferedQty(stLine.Reqid.Value, num);
                    }
                }
            }

            if (stLine.Mordlineid.HasValue)
            {
                decimal num2 = -stLine.Movqty.GetValueOrDefault();
                if (num2 != 0m)
                {
                    ManufactureSqlFunctions.MordlineIncreaseMovQty(stLine.Mordlineid.Value, num2);
                }
            }

            if (stLine.Pjpid.HasValue)
            {
                decimal num3 = -stLine.Movqty2.GetValueOrDefault();
                if (num3 != 0m)
                {
                    ProjPartsBL.IncreaseMovQty(stLine.Pjpid.Value, num3);
                }
            }

            DeleteCostLines(stLine);
            DeleteLotOrFifos(stLine);
        }
         
        public override void Delete(Key k)
        {
            int? resid = null;
            decimal? resqty = null;
 
            try
            {
                PreDeleteHeavyTools(k, out resid, out resqty);
                base.Delete(k);
            }
            catch (Exception e)
            {
                if (resid.HasValue && resqty.HasValue)
                {
                    var r = Base.Warehouse.Reserve.Reserve.Load(resid);
                    if (r != null)
                    {
                        r.Resqty = resqty;

                        var resBL = ReserveBL.New();
                        var map = resBL.CreateBLObjects();
                        map.Default = r;
                        resBL.Save(map);
                    }
                }
                throw e;
            } 
        } 
    }
}
