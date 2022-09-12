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
select pa.*,
       ph.pinvnum,
       cl.costtypeid, cl.realcostval as costval, realcurid as curid, cl.note as costlinenote,
       cu.name as apprusrid_name, rcu.name as replausrid_name
from ols_pinvapproval pa (nolock)
     left outer join ols_pinvhead ph (nolock) on ph.pinvid = pa.pinvid
     join cfw_user cu (nolock) on cu.usrid = pa.apprusrid
     left outer join cfw_user rcu (nolock) on rcu.usrid = pa.replausrid
     left outer join ols_costline cl (nolock) on cl.costlineid = pa.costlineid
";

        protected static QueryArg[] m_filters = new QueryArg[]
        {
            new QueryArg("pinvid", "pa", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("pinvnum", "ph", FieldType.String, QueryFlags.Like),
            new QueryArg("usrid", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("apprstat", "pa", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("padelstat", "delstat", "pa", FieldType.Integer, QueryFlags.Equals),
        };

        static ApprovedPinvLinePackageSearchProvider()
        {
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
        }

    }
}