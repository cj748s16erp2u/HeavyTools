using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageBL : DefaultBL2
    {
        public static readonly string ID = typeof(ApprovedPackageBL).FullName;

        public static ApprovedPackageBL New()
        {
            return (ApprovedPackageBL)ObjectFactory.New(typeof(ApprovedPackageBL));
        }
        protected ApprovedPackageBL() : base()
        {
        }

        public override DataRow GetInfo(IDictionary<string, object> args)
        {
            Key k = new Key(args);
            return GetInfo(k);
        }

        public DataRow GetInfo2(IDictionary<string, object> args)
        {
            Key k = new Key(args);
            return GetInfo2(k);
        }

        public static DataRow GetInfo(Key k)
        {
            string sql = @"
select prl.cmpcode, prl.prlcode, prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode as packagecode, prl.status
from oas_prllist prl (nolock)
where prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode=" + Utils.SqlToString(k["packagecode"]);

            DataRow row = new DataRow(SqlDataAdapter.GetSchema(CodaInt.Base.Module.CodaDBConnID, "ApprovedPackageInfoPart", sql));
            SqlDataAdapter.FillSingleRow(CodaInt.Base.Module.CodaDBConnID, row, sql);
            return row;
        }

        public static DataRow GetInfo2(Key k)
        {
            string sql = String.Format(
@"select pl.cmpcode, pl.prlcode, pl.pcmcode + '/' + pl.usrname + '/' + pl.prlcode as packagecode, prl.status--$$morefields$$
from /*u4findb*/..oas_prllist prl (nolock)
     left outer join /*u4findb*/..oas_prldetail pl (nolock) on pl.pcmcode + '/' + pl.usrname + '/' + pl.prlcode = prl.pcmcode + '/' + prl.usrname + '/' + prl.prlcode
     left outer join ofc_dochead dh (nolock) on dh.cmpcode = pl.cmpcode and dh.doccode = pl.doccode and dh.docnum = pl.docnum--$$morejoins$$
where prl.pcmcode+'/'+prl.usrname+'/'+prl.prlcode= {0}--$$morewhere$$",
Utils.SqlToString(k["packagecode"]));

            var dbLinkFin = DBConfig.GetDatabaseLink(Session.Catalog, CodaInt.Base.Module.CodaDBConnID);

            foreach (KeyValuePair<string, object> kv in k)
            {
                if (kv.Key == "pinvid")
                {
                    sql = sql.Replace("--$$morefields$$", @",
ph.pinvid, ph.pinvnum--$$morefields$$");
                    sql = sql.Replace("--$$morejoins$$", @"
left outer join ols_pinvhead as ph (nolock) on ph.pinvid = dh.pinvid--$$morejoins$$");
                    sql = sql.Replace("--$$morewhere$$", @"
 and ph.pinvid = " + Utils.SqlToString(kv.Value) + "--$$morewhere$$");
                }
            }

            sql = sql.Replace("/*u4findb*/", $"[{dbLinkFin.Database}]").
            Replace("--$$morefields$$", "").Replace("--$$morejoins$$", "").Replace("--$$morewhere$$", "");

            DataRow row = new DataRow(SqlDataAdapter.GetSchema(DB.Main, "ApprovedPackageInfoPart2", sql));
            SqlDataAdapter.FillSingleRow(DB.Main, row, sql);
            return row;
        }

    }
}
