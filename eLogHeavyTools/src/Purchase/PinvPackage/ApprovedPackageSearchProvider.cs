using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Lang;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ApprovedPackageSearchProvider).FullName;

        protected static string m_queryString = @"
select prl.cmpcode, prl.usrname, prl.prlcode, prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode as packagecode, 
     prl.paydate, prl.genhomenet, prl.homenet, prl.status,
	 case when b.curcode_db > 1 then null else prlsum.net_sum end net_summa,
	 case when b.curcode_db > 1 then '$more_curcode' else prlsum.curcode end curid
from /*u4findb*/..oas_prllist prl (nolock)
     outer apply (select max(prs.curcode) curcode, sum(prs.net) net_sum
                    from /*u4findb*/..oas_prlsumm prs (nolock) 
                   where prs.pcmcode+'/'+prs.usrname+'/'+prs.prlcode = prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode
                         and prs.deleted = 0) prlsum
     outer apply (select count(0) curcode_db
                    from (select prs.pcmcode+'/'+prs.usrname+'/'+prs.prlcode code, COUNT(0) db
                            from /*u4findb*/..oas_prlsumm prs (nolock)
                           where prs.pcmcode+'/'+prs.usrname+'/'+prs.prlcode = prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode
                                 and prs.deleted = 0
                        group by prs.pcmcode+'/'+prs.usrname+'/'+prs.prlcode, curcode
                        having count(curcode)>0) a
                 group by a.code) b";

        protected static QueryArg[] m_filters = new QueryArg[] {
            new QueryArg("cmpcode", "prl", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("packagecode", FieldType.String),
            new QueryArg("status", "prl", FieldType.Integer, QueryFlags.MultipleAllowed),
        };

        protected ApprovedPackageSearchProvider() : base(m_queryString, m_filters, SearchProviderType.Default, 1000)
        {
            ProcessRecordFunc = ProcessRecord;
        }

        protected void ProcessRecord(IDataReaderModifyable r)
        {
            var msgCurcode = eProjectWeb.Framework.ConvertUtils.ToString(r["curid"]);
            var args = new object[0];

            if (!string.IsNullOrEmpty(msgCurcode))
            {
                r["curid"] = string.Format(Translator.Translate(msgCurcode), args);
            }
        }

        static ApprovedPackageSearchProvider()
        {
            SetCustomFunc(m_filters, "packagecode", a => { a.Sb.AppendFormat(@"(rtrim(ltrim(prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode)) like '%' + {0} + '%')",
                eProjectWeb.Framework.Utils.SqlToString(a.ArgValue)); });
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
            query = query.Replace("/*u4findb*/", $"[{dbLinkFin.Database}]");
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            var dbLinkERP = DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.eLogDBConnID);
            var dbLinkFin = DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.CodaDBConnID);

            if (dbLinkERP.Server != dbLinkFin.Server)
            {
                using (new eProjectWeb.Framework.Lang.NS(this))
                {
                    CustomClientMessage = eProjectWeb.Framework.Lang.Translator.Translate("$db_not_same");
                    return;
                }
            }

            base.PreSearch(args);

            if (args == null)
            {
                args = new Dictionary<string, object>();
            }

            if (args.ContainsKey("cmpcode"))
            {
                var cmdCodes = new List<string>();
                var cmpcode = args["cmpcode"];
                if (cmpcode is string)
                    cmdCodes = new List<string>() { cmpcode.ToString() };
                else if (cmpcode is System.Collections.IEnumerable)
                    cmdCodes = new List<string>(((System.Collections.IEnumerable)cmpcode).Cast<object>().Select(x => x.ToString()));

                var sessionCmpCodes = Session.CompanyIds.Select(cmpId => eLog.Base.Setup.Company.CompanyCache.Get(cmpId)?.Codacode.ToStr()).Distinct().Where(cmpCode => !string.IsNullOrEmpty(cmpCode)).ToList();
                cmdCodes = sessionCmpCodes.Intersect(cmdCodes).ToList();
                args["cmpcode"] = cmdCodes;
            }
            else
            {
                var sessionCmpCodes = Session.CompanyIds.Select(cmpId => eLog.Base.Setup.Company.CompanyCache.Get(cmpId)?.Codacode.ToStr()).Distinct().Where(cmpCode => !string.IsNullOrEmpty(cmpCode)).ToList();
                args["cmpcode"] = sessionCmpCodes;
            }
        }

    }
}