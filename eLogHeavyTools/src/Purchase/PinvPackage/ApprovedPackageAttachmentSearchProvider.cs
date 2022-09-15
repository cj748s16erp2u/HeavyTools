using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageAttachmentSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ApprovedPackageAttachmentSearchProvider).FullName;

        protected static string m_queryString = @"
select 
oa.*,
pinvdoc.pinvnum as attdocnum, pinvdoc.pinvid, pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode as packagecode
from ols_attachment oa (nolock)
  outer apply (select ph.pinvid, ph.pinvnum, ph.ref1 from ols_pinvhead ph (nolock)
               where ph.pinvid = substring(oa.refid, CHARINDEX(':', oa.refid) + 1, LEN(oa.refid) - CHARINDEX(':', oa.refid) - 1 )) pinvdoc
     left outer join ofc_dochead dh (nolock) on dh.pinvid = pinvdoc.pinvid
     left outer join /*u4findb*/..oas_prldetail pl (nolock) on pl.cmpcode = dh.cmpcode and pl.doccode = dh.doccode and pl.docnum = dh.docnum
";

        protected static QueryArg[] m_filters = new QueryArg[]
        {
            new QueryArg("custom", FieldType.Variant),
        };

        protected ApprovedPackageAttachmentSearchProvider() : base(m_queryString, m_filters, SearchProviderType.Default, 1000)
        {
        }

        static ApprovedPackageAttachmentSearchProvider()
        {
            SetCustomFunc(m_filters, "custom", CustomFilter);
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
            var dbLinkFin = DBConfig.GetDatabaseLink(Session.Catalog, CodaInt.Base.Module.CodaDBConnID);
            query = query.Replace("/*u4findb*/", $"[{dbLinkFin.Database}]");
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

                    // ha van pinvid, akkor egy szamlahoz csatolt dokumentumok kellenek
                    if (pinvId.HasValue)
                    {
                        var sql = string.Empty;
                        StringBuilder sb2 = new StringBuilder();
                        sql = @"oa.src = 'pinvhead'
    and oa.refid ='{\""pinvid\"":'" + " + convert(varchar(10), " + eProjectWeb.Framework.Utils.SqlToString(pinvId) + ")" + "+ '}'" + " and oa.delstat = 0";
                        sb2.Append(sql);

                        QueryArg.BuildQueryArgString(sb2, arg, quotedFieldName, argValue);
                        sb.Append(sb2.ToString());
                    }
                    else
                    {
                        // ha nincs pinvid, akkor a csomaghoz tartozo osszes szamlahoz csatolt dokumentumok kellenek
                        if (!string.IsNullOrEmpty(packageCode))
                        {
                            sb.AppendFormat(@"(pl.pcmcode+'/'+pl.usrname+'/'+pl.prlcode = {0}) and oa.delstat = 0", eProjectWeb.Framework.Utils.SqlToString(packageCode));
                        }
                    }
                }
            }
        }

    }
}
