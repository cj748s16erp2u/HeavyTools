using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadSearchProvider3 : TransferingHeadSearchProvider {
        public TransferingHeadSearchProvider3()
        {
            var sql = @"select top $$top$$ sth.*,
       frompt.partncode fromptcode, frompt.name fromptname, fromaddr.name fromaddrname, fromaddr.add01 fromaddraddr1, fromwh.name fromwhname,
       topt.partncode toptcode, topt.name toptname, toaddr.name toaddrname, toaddr.add01 toaddraddr1, toaddr.extcode toaddrextcode, towh.name towhname,
       stdoc.name docname,sinv.docnum sinvdocnum,x.sinvexists, x.pinvexists,
       storno.docnum stornodocnum, orig.docnum origdocnum
       ,xl.linecount,xl.costval,xl.netval, xl.totval
       ,whs.fromwhid fromwhid2, whs.towhid towhid2
       ,prj.projcode, prj.name as projname, rosth.docnum retorigdocnum,

       --$$morefields$$
from ols_sthead sth (nolock)
     left join olc_sthead ch (nolock) on ch.stid=sth.stid
     join ols_stdoc stdoc (nolock) on stdoc.stdocid = sth.stdocid
     left outer join ols_partner frompt (nolock) on frompt.partnid = sth.frompartnid
     left outer join ols_partnaddr fromaddr (nolock) on fromaddr.addrid = sth.fromaddrid
     left outer join ols_partner topt (nolock) on topt.partnid = sth.topartnid
     left outer join ols_partnaddr toaddr (nolock) on toaddr.addrid = sth.toaddrid
     left outer join ols_sinvhead sinv (nolock) on sinv.sinvid = sth.sinvid
     left outer join ols_pinvhead pinv (nolock) on pinv.pinvid = sth.pinvid
     left outer join ols_sthead storno (nolock) on storno.stid = sth.corrstid
     left outer join ols_sthead orig (nolock) on orig.stid = sth.origstid
     left outer join ols_sthead rosth (nolock) on rosth.stid = sth.retorigstid
     outer apply ( select top 1 h2.fromwhid
                   from ols_stline l (nolock)
                          join ols_stline l2 (nolock) on l2.stlineid = l.intransitstlineid
                          join ols_sthead h2 (nolock) on h2.stid = l2.stid
                     where l.stid = sth.stid
                       and l.intransitstlineid is not null ) intr
     cross apply (select isnull(intr.fromwhid, sth.fromwhid) fromwhid, isnull(sth.intransittowhid, sth.towhid) towhid) whs
     left outer join ols_warehouse fromwh (nolock) on fromwh.whid = whs.fromwhid
     left outer join ols_warehouse towh (nolock) on towh.whid = whs.towhid
     left outer join ols_warehouse towh2 (nolock) on towh2.whid = isnull(ch.onroadtowhid, whs.towhid)
     outer apply (select (case when sth.corrtype=1 and sth.corrstid is not null then 0 when sth.corrtype=2 then 0 when sth.ststat<100 then 0 when stdoc.sinvdocid is null then 0 when sinv.nofprinted is null then 1 when sinv.nofprinted>=1 then 3 else 2 end) as sinvexists,
                         (case when sth.corrtype=1 and sth.corrstid is not null then 0 when sth.corrtype=2 then 0 when sth.ststat<100 then 0 when stdoc.pinvdocid is null then 0 when pinv.pinvid is null then 1 else 2 end) as pinvexists) x
     outer apply ( SELECT nullif(COUNT(0),0) as linecount, sum(costval) as costval, sum(invnetval) as netval, sum(invtotval) as totval
                   FROM ols_stline sl (nolock)
                   where sl.stid=sth.stid ) xl
     left outer join ols_project prj (nolock) on prj.projid = sth.projid
     --$$morejoins$$
where ( (whs.fromwhid is not null and exists( select 0 from ols_userwh uw (nolock) where uw.usrid = $$usrid$$ and uw.whid = whs.fromwhid and (uw.uwrtype & $$uwrtype$$) <> 0 )) or
        (whs.towhid is not null and exists( select 0 from ols_userwh uw (nolock) where uw.usrid = $$usrid$$ and uw.whid = whs.towhid and (uw.uwrtype & $$uwrtype$$) <> 0 )) )
  and exists( select 0 from ols_userstdoc us (nolock) where us.usrid = $$usrid$$ and us.stdocid = sth.stdocid )
";
        }
        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var sql=base.CreateQueryString(args, fmtonly);

            return sql.
                Replace("--$$morejoins$$",
                    @"left join olc_sthead ch (nolock) on ch.stid=sth.stid 
                      left join ols_warehouse towh2 (nolock) on towh2.whid = isnull(ch.onroadtowhid, whs.towhid)").
                Replace("--$$morefields$$", ",towh2.whid towh2whid, towh2.name towh2name");
        }
    }
}
