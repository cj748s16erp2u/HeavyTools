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
    public class ApprovedPinvHeadPackageSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ApprovedPinvHeadPackageSearchProvider).FullName;

        protected static string m_query = @"
select pl.cmpcode, pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode as packagecode,pl.doccode, pl.docnum, pl.net, pl.descr, pl.extref1, 
       ph.pinvnum, 
       dh.pinvid
from /*u4findb*/..oas_prldetail pl (nolock)
     left outer join ofc_dochead as dh (nolock) on dh.cmpcode = pl.cmpcode and dh.doccode = pl.doccode and dh.docnum = pl.docnum
     left outer join ols_pinvhead as ph (nolock) on ph.pinvid = dh.pinvid
";

        protected static QueryArg[] m_filters = new QueryArg[]
        {
            new QueryArg("packagecode", FieldType.String),
            new QueryArg("pinvnum", "ph", FieldType.String, QueryFlags.Like),
        };

        static ApprovedPinvHeadPackageSearchProvider()
        {
            SetCustomFunc(m_filters, "packagecode", a => {
                a.Sb.AppendFormat(@"(rtrim(ltrim(pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode)) = {0})",
                    eProjectWeb.Framework.Utils.SqlToString(a.ArgValue));
            });
        }

        protected ApprovedPinvHeadPackageSearchProvider() : base(m_query, m_filters, SearchProviderType.Default, 1000)
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
