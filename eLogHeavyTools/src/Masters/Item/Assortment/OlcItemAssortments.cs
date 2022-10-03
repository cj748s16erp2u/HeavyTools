using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Assortment
{
    public class OlcItemAssortments : EntityCollection<OlcItemAssortment, OlcItemAssortments>
    {
        public static OlcItemAssortments Load(DBConnID connID, string sql)
        {
            var lines = New();
            SqlDataAdapter.FillDataSet(connID, lines, sql);
            return lines;
        }
    }
}
