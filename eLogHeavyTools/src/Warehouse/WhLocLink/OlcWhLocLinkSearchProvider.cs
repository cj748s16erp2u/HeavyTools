using eProjectWeb.Framework.Data;
using Interf;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkSearchProvider : DefaultSearchProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkSearchProvider).FullName;

        protected static string queryString = @"
select [wll].*, [whz].[whzonecode], [whz].[name] [whzname], [whl].[whloccode], [whl].[name] [whlname], [wh].[name] [whname], [wlll].* 
from [olc_whloclink] [wll] (nolock)
  left join [olc_whzone] [whz] (nolock) on [whz].[whzoneid] = [wll].[whzoneid]
  join [olc_whlocation] [whl] (nolock) on [whl].[whlocid] = [wll].[whlocid]
  join [ols_warehouse] [wh] (nolock) on [wh].[whid] = [wll].[whid]
  outer apply (select sum([wl].[volume]) [volume]
    from [olc_whloclinkline] [wlll] (nolock)
      join [olc_whlocation] [wl] (nolock) on [wl].[whlocid] = [wlll].[whlocid]
    where [wlll].[whllid] = [wll].[whllid]) [wlll]";

        protected static QueryArg[] argsTemplate = new QueryArg[]
        {
            new QueryArg("whllid", "wll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whid", "wll", FieldType.String, QueryFlags.Like),
            new QueryArg("whzoneid", "wll", FieldType.Integer, QueryFlags.MultipleAllowed),
            new QueryArg("whloccode", "whl", FieldType.String, QueryFlags.Like),
            new QueryArg("whlname","name", "whl", FieldType.String, QueryFlags.Like),
            new QueryArg("startdate", "enddate", "wll", FieldType.DateTime, QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("enddate", "startdate", "wll", FieldType.DateTime, QueryFlags.Less | QueryFlags.Equals)
        };

        protected OlcWhLocLinkSearchProvider() : base(queryString, argsTemplate, SearchProviderType.Default)
        {

        }

    }
}
