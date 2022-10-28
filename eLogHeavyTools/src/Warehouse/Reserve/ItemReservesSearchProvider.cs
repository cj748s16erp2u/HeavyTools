using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Warehouse.Reserve
{
    internal class ItemReservesSearchProvider3 : DefaultSearchProvider
    {
        public static readonly string ID = typeof(ItemReservesSearchProvider3).FullName;

        protected static string m_queryString =
@"SELECT * FROM (
  SELECT sh.cmpid, sl.itemid, $$restype_1$$ AS reservetype, sh.sorddocid AS docid, sh.docnum, sh.sorddate as rdate, r.whid, sl.linenum, p.partncode as ptwhcode, p.name as ptwhname, pa.name as ptaddrname, null as note, sl.ordqty, r.resqty
  FROM ols_sordline sl (nolock)
       left join olc_sordline_res csl (nolock) on csl.sordlineid=sl.sordlineid
       JOIN ols_reserve r (nolock) ON r.resid = sl.resid or r.resid = csl.resid 
       JOIN ols_sordhead sh (nolock) ON sh.sordid = sl.sordid
       JOIN ols_partner p (nolock) ON sh.partnid = p.partnid
       JOIN ols_partnaddr pa (nolock) ON sh.addrid = pa.addrid
  WHERE r.resqty > 0
    AND sl.itemid = $$itemid$$

  UNION ALL

  SELECT r.cmpid, r.itemid, $$restype_2$$ AS reservetype, null docid, convert(varchar, r.resid) docnum, r.resdate as rdate, r.whid, null linenum, p.partncode as ptwhcode, p.name as ptwhname, pa.name as ptaddrname, r.note as note, null ordqty, r.resqty
  FROM ols_reserve r (nolock)
       JOIN ols_partner p (nolock) ON r.partnid = p.partnid
       JOIN ols_partnaddr pa (nolock) ON r.addrid = pa.addrid
  WHERE not exists(select 0 from ols_sordline sl (nolock) where sl.resid = r.resid)
    AND r.resqty > 0
    AND r.itemid = $$itemid$$

  UNION ALL

  SELECT sh.cmpid, sl.itemid, $$restype_3$$ AS reservetype, sh.stdocid AS docid, sh.docnum, sh.stdate as rdate, sh.fromwhid as whid, sl.linenum, p.partncode as ptwhcode, p.name as ptwhname, pa.name as ptaddrname, sl.note, sl.ordqty, sl.dispqty as resqty
  FROM ols_stline sl (nolock)
       JOIN ols_sthead sh (nolock) ON sh.stid = sl.stid
       JOIN ols_partner p (nolock) ON sh.topartnid = p.partnid
       JOIN ols_partnaddr pa (nolock) ON sh.toaddrid = pa.addrid
  WHERE sh.sttype = $$sttype_is$$ -- kivet
    AND sh.ststat < $$stclosed$$
    AND sl.dispqty > 0
    AND sl.itemid = $$itemid$$

  UNION ALL

  SELECT sh.cmpid, sl.itemid, $$restype_4$$ AS reservetype, sh.stdocid AS docid, sh.docnum, sh.stdate as rdate, sh.fromwhid as whid, sl.linenum, sh.towhid as ptwhcode, w.name as ptwhname, convert(varchar(100), null) as ptaddrname, sl.note, sl.ordqty, sl.dispqty as resqty
  FROM ols_stline sl (nolock)
       JOIN ols_sthead sh (nolock) ON sh.stid = sl.stid
       JOIN ols_warehouse w (nolock) ON w.whid = sh.towhid
  WHERE sh.sttype = $$sttype_tr$$ -- raktarkozi
    AND sh.ststat < $$stclosed$$
    AND sl.dispqty > 0
    AND sl.itemid = $$itemid$$
) x
";

        protected static QueryArg[] m_argsTemplate = new QueryArg[] {
            new QueryArg("cmpid", FieldType.Integer, QueryFlags.BitwiseAnd),
            new QueryArg("itemid", FieldType.Integer),
            new QueryArg("whid", FieldType.String),
        };

        protected ItemReservesSearchProvider3()
            : base(m_queryString, m_argsTemplate, SearchProviderType.Default, 200)
        {
        }

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            string s = base.CreateQueryString(args, fmtonly);

            int itemId = 0;
            if (args.ContainsKey("itemid"))
                itemId = ConvertUtils.ToInt32(args["itemid"]).GetValueOrDefault();

            s = s.Replace("$$itemid$$", Utils.SqlToString(itemId));
            s = s.Replace("$$sttype_tr$$", Utils.SqlToString((int)eLog.Base.Warehouse.StockTran.StDocType.Transfering));
            s = s.Replace("$$sttype_is$$", Utils.SqlToString((int)eLog.Base.Warehouse.StockTran.StDocType.Issuing));
            s = s.Replace("$$stclosed$$", Utils.SqlToString((int)eLog.Base.Warehouse.StockTran.StHeadStStatList.Values.Closed));

            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("[$][$]restype_([0-9]+)[$][$]"); // $$restype_1$$, $$restype_10$$
            using (eProjectWeb.Framework.Lang.NS ns = new eProjectWeb.Framework.Lang.NS(this))
                s = rx.Replace(s, delegate (System.Text.RegularExpressions.Match m) { return Utils.SqlToString(eProjectWeb.Framework.Lang.Translator.Translate("$restype_" + m.Groups[1])); });

            return s;
        }

        protected override void PreSearch(Dictionary<string, object> args)
        {
            base.PreSearch(args);

            if (args.ContainsKey("sordlineid"))
            {
                string sql2 = String.Format(@"
                    SELECT sh.whid,sl.itemid
                    FROM ols_sordline sl
                    JOIN ols_sordhead sh ON sh.sordid=sl.sordid
                    WHERE sl.sordlineid={0}
                ", Utils.SqlToString(args["sordlineid"]));

                DataSet ds1 = SqlDataAdapter.Query(DB.Main, sql2);
                if (ds1.AllRows.Count != 1)
                    return;
                DataRow dr2 = ds1.AllRows[0];
                int? itemid = ConvertUtils.ToInt32(dr2["itemid"]);
                string whid = ConvertUtils.ToString(dr2["whid"]);

                args["itemid"] = itemid;
                args["whid"] = whid;
            }
            else if (args.ContainsKey("poglineid"))
            {
                string sql2 = String.Format(@"
                    SELECT pogh.whid,pogl.itemid
                    FROM ols_pogline pogl
                    JOIN ols_poghead pogh ON pogh.pogid=pogl.pogid
                    WHERE pogl.poglineid={0}
                ", Utils.SqlToString(args["poglineid"]));

                DataSet ds1 = SqlDataAdapter.Query(DB.Main, sql2);
                if (ds1.AllRows.Count != 1)
                    return;
                DataRow dr2 = ds1.AllRows[0];
                int? itemid = ConvertUtils.ToInt32(dr2["itemid"]);
                string whid = ConvertUtils.ToString(dr2["whid"]);

                args["itemid"] = itemid;
                args["whid"] = whid;
            }


            if (args.ContainsKey("allwh"))
            {
                int? v = ConvertUtils.ToInt32(args["allwh"]);
                if (v == 1)
                {
                    args.Remove("whid");
                }
            }

            int cmpid = -1;
            if (args.ContainsKey("cmpid"))
                cmpid = Convert.ToInt32(args["cmpid"]);
            else
                cmpid = -1;
            args["cmpid"] = (eProjectWeb.Framework.Session.CompanyFlags & cmpid);
        }
    }
}
