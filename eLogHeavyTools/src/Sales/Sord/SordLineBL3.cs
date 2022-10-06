using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineBL3 : SordLineBL
    {
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcSordLine>();
            if (olc != null)
            {
                RuleServer.Validate<OlcSordLine, OlcSordLineRules>(objects);
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcSordLine)
            {
                var sordLine = objects.Get<SordLine>();
                ((OlcSordLine)e).Sordlineid = sordLine.Sordlineid;
            }

            return base.PreSave(objects, e);
        }

        protected override void PreDelete(Key k)
        {
            
            var sl = SordLine.Load(k);
            var sh = SordHead.Load(sl.Sordid);

            if (sl.Resid.HasValue && sh.Sordtype == 2)
            {
                foreach (var row in SqlDataAdapter.Query($@"select lll.ordqty-isnull(anotherresqty,0) newresqty, r.resid
                        from ols_sordline ll
                        join olc_sordline cc on cc.sordlineid=ll.sordlineid
                        join ols_sordline lll on lll.sordlineid=cc.preordersordlineid
                        left join ols_reserve r on r.resid=lll.resid
                        outer apply (
	                    select sum( isnull(dispqty,0)+isnull(resqty,0)) anotherresqty
	                        from olc_sordline c
	                        join ols_sordline l on l.sordlineid=c.sordlineid
	                        left join ols_reserve r on r.resid=l.resid
	                        outer apply (
		                    select ordqty, dispqty
		                        from ols_stline st
		                        where st.sordlineid=l.sordlineid
	                        ) x
	                        where c.preordersordlineid=lll.sordlineid
	                        and c.sordlineid<>ll.sordlineid
                        ) x
                        where ll.sordlineid={sl.Sordlineid}").AllRows)
                {
                    var newResQty = ConvertUtils.ToDecimal(row["newresqty"]);
                    var resid = ConvertUtils.ToInt32(row["resid"]);

                    if (newResQty.HasValue)
                    {
                        var r = Reserve.Load(resid);
                        r.Resqty = newResQty.Value;
                        var bl = ReserveBL.New();
                        var map = bl.CreateBLObjects();
                        map.Default = r;
                        bl.Save(map);
                    }
                    break;
                }
            }

            var olc = OlcSordLine.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.PreDelete(k);
        }
    }
}