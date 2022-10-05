using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.StockTran;
using eLog.HeavyTools.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    public class IssuingHeadBL3 : IssuingHeadBL
    {
        protected override int AfterClose(AfterCloseArgs args)
        {
            var i = base.AfterClose(args);
            if (i == 0) {
                var lst = StLines.Load(args.Sthead.PK);

                foreach (StLine item in lst.AllRows)
                {
                    if (item.Sordlineid.HasValue)
                    {
                        var sl = SordLine.Load(item.Sordlineid);
                        var sh = SordHead.Load(sl.Sordid);

                        if (sh.Sordtype == 2)
                        {
                            var csl = OlcSordLine.Load(sl.Sordlineid);

                            if (csl.Preordersordlineid.HasValue)
                            {
                                var movqty = ConvertUtils.ToDecimal(SqlDataAdapter.ExecuteSingleValue(DB.Main, 
$@"select sum(l.movqty) movqty
  from olc_sordline c
  join ols_stline l on l.sordlineid=c.sordlineid
  where preordersordlineid={csl.Preordersordlineid}"));

                                var sordline = SordLine.Load(csl.Preordersordlineid);
                                sordline.Movqty = movqty;
                                sordline.Save();


                            }
                             
                        }
                    }
                }
            }


            return i;
        }
    }
}
