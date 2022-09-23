using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpCustSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcPartnCmpCustSearchProvider).FullName;

        protected static string m_query =
@"select opc.partnid, opc.cmpid, opc.relatedaccno, opc.scontoinvoice, opc.scontobelowaccno, opc.scontoaboveaccno,
     opc.el2, opc.el3, opc.transactionfeeaccno, opc.domesticvaluerate, opc.referencetype, 
     opc.discountaccounting, opc.valuecurid,
     c.codacode c_codacode, c.abbr c_abbr--$$morefields$$
from ols_partncmp pc (nolock)
     left outer join olc_partncmp opc (nolock) on pc.partnid = opc.partnid and opc.cmpid = pc.cmpid
     left outer join ols_company c (nolock) on c.cmpid = opc.cmpid
     left outer join ols_partner p (nolock) on p.partnid = pc.partnid--$$morejoins$$
";

        protected static QueryArg[] m_filters = new QueryArg[]
            {
                new QueryArg("partnid", "opc", FieldType.Integer, QueryFlags.Equals),
                new QueryArg("cmpid", "c", FieldType.Integer, QueryFlags.Equals | QueryFlags.MultipleAllowed),
                new QueryArg("cmpcode", "c", FieldType.String, QueryFlags.MultipleAllowed),
            };

        static OlcPartnCmpCustSearchProvider()
        {
        }

        protected OlcPartnCmpCustSearchProvider() : base(m_query, m_filters, SearchProviderType.Default, 1000)
        {
            SetCustomFunc("cmpcode", eLog.Base.Common.CommonUtils.CmpCodesCustomFilter);
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var sql = base.CreateQueryString(args, fmtonly);
            ModifyQueryString(args, fmtonly, ref sql);

            sql = sql.Replace("--$$morefields$$", "").Replace("--$$morejoins$$", "");

            return sql;
        }

        protected virtual void ModifyQueryString(Dictionary<string, object> args, bool fmtonly, ref string sql)
        {
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);

            eLog.Base.Common.CommonUtils.AddSessionCompaniesFilter(args, "cmpid");
        }

    }
}
