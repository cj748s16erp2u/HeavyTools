using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework.Data;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eLog.Base;

namespace eLog.HeavyTools.Warehouse.Reserve
{
    internal class ReserveSearchProvider3 : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ReserveSearchProvider3).FullName;

        protected static string m_queryString =
@"select r.*,
         i.itemcode, i.name01 itemname01, i.name03 itemname03,
         p.partncode partncode, p.name partnname, w.name whname, sl.sordid, sl.sordlineid, sh.docnum as sorddocnum,
         pa.extcode partnaddr_extcode, pa.name+' '+pa.add01+' '+pa.add02 partnaddr_name,
         lot.lotnum, lot.ucdid, uqcd.uccode, r1.reqid partreqid, mh1.docnum + '/' + r1.lev partreqlev, r2.reqid fromreqid, mh2.docnum + '/' + r2.lev fromreqlev
from ols_reserve r (nolock)
     join ols_item i (nolock) on r.itemid = i.itemid
     join ols_warehouse w (nolock) on r.whid = w.whid
     join ols_partner p (nolock) on r.partnid = p.partnid
     join ols_partnaddr pa (nolock) on r.addrid = pa.addrid
	 left join olc_sordline_res slr (nolock) on slr.resid=r.resid
     left join ols_sordline sl (nolock) on r.resid = sl.resid or sl.sordlineid=slr.sordlineid
     left join ols_sordhead sh (nolock) on sh.sordid = sl.sordid
     left outer join ols_lot lot (nolock) on lot.lotid = r.lotid
     left outer join ols_uqcard uqcd (nolock) on uqcd.ucdid = lot.ucdid
     -- mord - req - partresid
     left join ols_requirement r1 (nolock)
     join ols_procsequence ps1 (nolock) on ps1.procseqid = r1.procseqid
     join ols_mordline ml1 (nolock) on ml1.mordlineid = ps1.mordlineid
     join ols_mordhead mh1 (nolock) on mh1.mordid = ml1.mordid
       on r1.partresid = r.resid
     -- mord - req - fromresid
     left join ols_requirement r2 (nolock)
     join ols_procsequence ps2 (nolock) on ps2.procseqid = r2.procseqid
     join ols_mordline ml2 (nolock) on ml2.mordlineid = ps2.mordlineid
     join ols_mordhead mh2 (nolock) on mh2.mordid = ml2.mordid
       on r2.fromresid = r.resid
";

        protected static QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("resid", "r", FieldType.Integer), // 0
            new QueryArg("cmpid", "r", FieldType.Integer, QueryFlags.BitwiseAnd),
            new QueryArg("itemcode", "i", FieldType.String, QueryFlags.Like),
            new QueryArg("name01", "i", FieldType.String, QueryFlags.Like),
            new QueryArg("name03", "i", FieldType.String, QueryFlags.Like),
            new QueryArg("partncode", "p", FieldType.String, QueryFlags.Like),
            new QueryArg("partnname", "name","p", FieldType.String, QueryFlags.Like),
            new QueryArg("whid","w", FieldType.String, QueryFlags.MultipleAllowed),
            new QueryArg("restype", "r",  FieldType.Integer,QueryFlags.MultipleAllowed),
            new QueryArg("partnaddr_extcode","extcode", "pa", FieldType.String,QueryFlags.None),
            new QueryArg("partnaddr_name","name", "pa", FieldType.String,QueryFlags.Like),
            new QueryArg("partnaddr_addr1","add01", "pa", FieldType.String,QueryFlags.Like),
            new QueryArg("partnaddr_addr2","add02", "pa", FieldType.String,QueryFlags.Like),
            new QueryArg("note", "r",  FieldType.String, QueryFlags.Like),
            new QueryArg("usrid", FieldType.String),
            new QueryArg("sorddocnum","docnum","sh", FieldType.String, QueryFlags.Like),
            new QueryArg("nonzero", FieldType.Integer),
            new QueryArg("lotid", "r", FieldType.Integer),
            new QueryArg("lotnum", "lot", FieldType.String),
            new QueryArg("lotnum_like", "lotnum", "lot", FieldType.String, QueryFlags.Like),
            new QueryArg("ucdid", "lot", FieldType.Integer),
            new QueryArg("uccode", "uqcd", FieldType.String),
            new QueryArg("uccode_like", "uccode", "uqcd", FieldType.String, QueryFlags.Like),
        };

        protected ReserveSearchProvider3()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default, 200)
        {
            SetCustomFunc("usrid", UserIdCustomFilter);
            SetCustomFunc("nonzero", NonZeroCustomFilter);
        }

        protected void UserIdCustomFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            string s = string.Format("exists( select 0 from ols_userwh uw (nolock) where uw.usrid = {0} and uw.whid = w.whid and (uw.uwrtype & {1}) <> 0 )", Utils.SqlToString(argValue), Utils.SqlToString((int)eLog.Base.Maintenance.UserWh.UserWhUWRType.STUpdate));
            sb.Append(s);
        }

        public void NonZeroCustomFilter(StringBuilder sb, QueryArg arg, string quotedFieldName, object argValue)
        {
            bool nonzero;
            if (argValue is bool)
                nonzero = (bool)argValue;
            else
                nonzero = ((argValue as int?).GetValueOrDefault() != 0);

            if (nonzero)
                sb.Append("r.resqty <> 0");
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);

            if (args == null)
                args = new Dictionary<string, object>();

            args["usrid"] = eProjectWeb.Framework.Session.UserID;

            int cmpid = -1;
            if (args.ContainsKey("cmpid"))
                cmpid = Convert.ToInt32(args["cmpid"]);
            else
                cmpid = -1;
            args["cmpid"] = (eProjectWeb.Framework.Session.CompanyFlags & cmpid);
        }

        protected override void SetupColumns(ColumnCollection schema)
        {
            base.SetupColumns(schema);

            foreach (Column col in schema)
            {
                if (col.FieldName.Equals("sordlineid", StringComparison.OrdinalIgnoreCase)) // a sordlineid az ols_sordline tablaban valoban PK, de itt nem
                    col.PKFieldName = "";
            }

            if (!ModuleParts.UseManufacture)
            {
                HideColumn(schema, "partreqid");
                HideColumn(schema, "partreqlev");
                HideColumn(schema, "fromreqid");
                HideColumn(schema, "fromreqlev");
            }
        }
    }
}
