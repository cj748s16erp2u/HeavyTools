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

    }
}
