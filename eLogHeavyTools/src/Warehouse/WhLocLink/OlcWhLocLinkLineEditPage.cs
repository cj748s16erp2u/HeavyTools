using eProjectWeb.Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    internal class OlcWhLocLinkLineEditPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(OlcWhLocLinkLineEditPage).FullName;
        public OlcWhLocLinkLineEditPage() : base("Loc")
        {
            Tabs.AddTab(() => OlcWhLocLinkLineEditTab.New(OlcWhLocLinkSearchPage.SetupLine));
        }
    }
  
}
