using eLog.Base.Masters.Item;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    public class ItemCmps2 : ItemCmps
    {
        public static ItemCmps LoadAll(Key k)
        {
            ItemCmps ic = new ItemCmps();
            string querySQL = "select * from ols_itemcmp where " + k.ToSql();
            SqlDataAdapter.FillDataSet(DB.Main, ic, querySQL);
            return ic;
        }
    }
}
