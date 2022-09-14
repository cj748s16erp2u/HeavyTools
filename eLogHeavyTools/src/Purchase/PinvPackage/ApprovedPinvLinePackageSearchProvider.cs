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
select distinct pa.*, pa.note as apprnote,
       ph.pinvnum,
       cl.costtypeid, cl.realcostval as costval, realcurid as curid, cl.note as costlinenote,
       cu.name as apprusrid_name, rcu.name as replausrid_name
from ols_pinvapproval pa (nolock)
     left outer join ols_pinvhead ph (nolock) on ph.pinvid = pa.pinvid
     join cfw_user cu (nolock) on cu.usrid = pa.apprusrid
     left outer join cfw_user rcu (nolock) on rcu.usrid = pa.replausrid
     left outer join ols_costline cl (nolock) on cl.costlineid = pa.costlineid
     left outer join ofc_dochead dh (nolock) on dh.pinvid = pa.pinvid
     left outer join /*u4findb*/..oas_prldetail pl on pl.cmpcode = dh.cmpcode and pl.doccode = dh.doccode and pl.docnum = dh.docnum
";

        protected static QueryArg[] m_filters = new QueryArg[]
        {
            new QueryArg("pinvnum", "ph", FieldType.String, QueryFlags.Like),
            new QueryArg("usrid", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("apprstat", "pa", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("padelstat", "delstat", "pa", FieldType.Integer, QueryFlags.Equals),
            new QueryArg("custom", FieldType.Variant),
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
            SetCustomFunc(m_filters, "custom", CustomFilter);
        }

        protected ApprovedPinvLinePackageSearchProvider() : base(m_query, m_filters, SearchProviderType.Default, 1000)
        {
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            args["custom"] = args;

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

        private static void CustomFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            int? pinvId = null;
            string packageCode = "";

            if (argValue != null)
            {
                if (argValue is System.Collections.IEnumerable)
                {
                    if (((Dictionary<string, object>)argValue).ContainsKey("selectedpinvid"))
                    {
                        pinvId = ConvertUtils.ToInt32(((Dictionary<string, object>)argValue)["selectedpinvid"]);
                    }
                    if (((Dictionary<string, object>)argValue).ContainsKey("selectedpackagecode"))
                    {
                        packageCode = ConvertUtils.ToString(((Dictionary<string, object>)argValue)["selectedpackagecode"]);
                    }

                    // ha van pinvid, akkor egy szamlahoz kapcsolodo tetelek kellenek
                    if (pinvId.HasValue)
                    {
                        sb.AppendFormat(@"(pa.pinvid = {0})", pinvId);
                    }
                    else
                        // ha nincs pinvid, akkor a csomaghoz tartozo osszes szamlahoz kapcsolodo tetelek kellenek
                        if (!string.IsNullOrEmpty(packageCode))
                        {
                            sb.AppendFormat(@"(pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode = {0})", eProjectWeb.Framework.Utils.SqlToString(packageCode));
                        }
                }
            }
        }

    }
}