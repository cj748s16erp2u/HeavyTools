using eLog.Base.Common;
using eLog.Base.Warehouse.StockMap;
using eLog.HeavyTools.Warehouse.WhZStockMap;
using eLog.HeavyTools.Warehouse.WhLocPrio;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Warehouse.WhZStockMap
{
    public class WhZStockMapSearchTab : SearchTabTemplate1
    {
        public static WhZStockMapSearchTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<WhZStockMapSearchTab>();
            t.Initialize(nameof(WhZStockMapDto), setup, DefaultActions.None);
            return t;
        }

        protected Control ctrlItemCode;
        protected Control ctrlItemname;
        protected Control ctrlWhid;
        protected Control ctrlWhzoneid;
        protected Control ctrlWhlocid;
        protected WhZStockMapSearchTab()
        {
            RequiredListNames.Add(eLog.Base.Setup.Company.CompanyGrantedList.ID);
        }

        protected override void CreateBase()
        {
            base.CreateBase();
            ctrlItemCode = this.SrcBar["Itemcode"];
            ctrlItemname = this.SrcBar["Itemname"];
            ctrlWhid = this.SrcBar["Whid"];
            ctrlWhzoneid = this.SrcBar["Whzoneid"];
            ctrlWhlocid = this.SrcBar["Whlocid"];
        }
    }
}
