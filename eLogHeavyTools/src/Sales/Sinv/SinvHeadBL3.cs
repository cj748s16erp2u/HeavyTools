using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Sales.Sinv
{
    public class SinvHeadBL3 : CodaInt.Base.Sales.Sinv.SinvHeadBL2
    {
        public byte[] GetPrintedPdf(int sinvId)
        {
            return GetPrintedPdfs(new int[] { sinvId }).Values.FirstOrDefault();
        }

        public Dictionary<int, byte[]> GetPrintedPdfs(IEnumerable<int> sinvIds)
        {
            var dict = new Dictionary<int, byte[]>();
            if (sinvIds.Any())
            {
                var x = sinvIds.Select(sinvId => new { SinvId = sinvId, SdDocId = sinvId.ToString("g") }).ToList();
                var sql = "select id, data from cfw_storeddoc where name = 'sinvpdf' and id in (" + Utils.SqlInToString(x.Select(x1 => x1.SdDocId)) + ")";
                Base.Common.SqlFunctions.QueryData(eProjectWeb.Framework.Data.DB.Main, sql, dr =>
                {
                    var id = Convert.ToString(dr["id"]);
                    var sinvId = x.First(x1 => x1.SdDocId == id).SinvId;
                    dict[sinvId] = (byte[])dr["data"];
                });
            }

            return dict;
        }
    }
}
