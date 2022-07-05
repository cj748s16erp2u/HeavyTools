using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Season
{
    class OlcItemSeasons : EntityCollection<OlcItemSeason, OlcItemSeasons>
    {
        internal static OlcItemSeasons LoadOrderBy(Key k)
        {
            var sql = string.Format("select top 20 * from {0} (nolock) where {1} order by substring(isid,2,2) desc , substring(isid,1,1)", Entity<OlcItemSeason>._TableName, k.ToSql());

            var lines = New();
            SqlDataAdapter.FillDataSet(Entity<OlcItemSeason>.Descriptor.DBConnID, lines, sql);

            return lines; 
        }
    }
}