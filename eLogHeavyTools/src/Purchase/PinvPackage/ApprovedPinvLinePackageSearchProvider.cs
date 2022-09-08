using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPinvLinePackageSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ApprovedPinvLinePackageSearchProvider).FullName;

        protected static string m_query = @"
select pl.cmpcode, pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode as packagecode,pl.doccode, pl.docnum, pl.net, pl.descr, pl.extref1, 
       ph.pinvnum, 
       dh.pinvid,
       pa.seqno, pa.apprstat, pa.apprdate, pa.apprusrid, pa.replausrid, pa.note as apprnote, pa.delstat padelstat,
       cl.costtypeid, cl.realcostval as costval, realcurid as curid, cl.note as costlinenote,
       cu.name as apprusrid_name, rcu.name as replausrid_name
from /*u4findb*/..oas_prldetail pl (nolock)
     left outer join ofc_dochead as dh (nolock) on dh.cmpcode = pl.cmpcode and dh.doccode = pl.doccode and dh.docnum = pl.docnum
     left outer join ols_pinvhead as ph (nolock) on ph.pinvid = dh.pinvid
     left outer join ols_pinvline pli (nolock) on pli.pinvid = ph.pinvid
     left outer join ols_pinvapproval pa (nolock) on pa.pinvid = ph.pinvid
     left outer join ols_costline cl (nolock) on cl.costlineid = pa.costlineid
     join cfw_user cu (nolock) on cu.usrid = pa.apprusrid
     left outer join cfw_user rcu (nolock) on rcu.usrid = pa.replausrid
";

        protected static QueryArg[] m_filters = new QueryArg[]
        {
            new QueryArg("packagecode", FieldType.String),
            new QueryArg("pinvnum", "ph", FieldType.String, QueryFlags.Like),
            new QueryArg("usrid", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("apprstat", "pa", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("padelstat", "delstat", "pa", FieldType.Integer, QueryFlags.Equals),
        };

        static ApprovedPinvLinePackageSearchProvider()
        {
            SetCustomFunc(m_filters, "packagecode", a => {
                a.Sb.AppendFormat(@"(rtrim(ltrim(pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode)) = {0})", 
                    eProjectWeb.Framework.Utils.SqlToString(a.ArgValue));
            });
            SetCustomFunc(m_filters, "usrid", a =>
            {
                var sb = new StringBuilder();
                QueryArg.BuildQueryArgString(sb, a.Arg, "\t", a.ArgValue);
                if (sb.Length > 0)
                    a.Sb.Append($"({sb.ToString().Replace("\t", "pa.apprusrid")} or {sb.ToString().Replace("\t", "pa.replausrid")})");
            });
        }

        protected ApprovedPinvLinePackageSearchProvider() : base(m_query, m_filters, SearchProviderType.Default, 1000)
        {
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var sql = base.CreateQueryString(args, fmtonly);
            ModifyQueryString(args, fmtonly, ref sql);

            return sql;
        }

        protected virtual void ModifyQueryString(Dictionary<string, object> args, bool fmtonly, ref string query)
        {
            var dbLinkFin = DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.CodaDBConnID);

            var dbLinkFIN = DBConfig.GetDatabaseLink(Session.Catalog, CodaInt.Base.Module.CodaDBConnID);
            query = query.Replace("/*u4findb*/", $"[{dbLinkFIN.Database}]");
        }

    }
}
