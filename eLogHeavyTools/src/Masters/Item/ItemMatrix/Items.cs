﻿using eLog.HeavyTools.Masters.Item.Model;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.ItemMatrix
{
    public class Items : EntityCollection<Base.Masters.Item.Item, Items>
    {
        public static Items Load(DBConnID connID, string sql)
        {
            var lines = New();
            SqlDataAdapter.FillDataSet(connID, lines, sql);
            return lines;
        }

    }
}