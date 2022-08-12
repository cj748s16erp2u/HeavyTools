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

        protected static string m_queryString = @"
select prl.cmpcode, prl.usrname, prl.prlcode, prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode as packagecode, 
     prl.paydate, prl.genhomenet, prl.homenet
from /*u4findb*/..oas_prllist prl (nolock)
";

        protected static QueryArg[] m_filters = new QueryArg[] {
            new QueryArg("cmpcode", "prl", FieldType.String, QueryFlags.MultipleAllowed),
        };

        protected ApprovedPinvHeadPackageSearchProvider() : base(m_queryString, m_filters, SearchProviderType.Default, 1000)
        {
        }

        static ApprovedPinvHeadPackageSearchProvider()
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