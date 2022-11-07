using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    public class ReceivingHeadSearchPage3 : Base.Warehouse.StockTran.ReceivingHeadSearchPage
    {
        private static Type baseType = typeof(Base.Warehouse.StockTran.ReceivingHeadSearchPage);

        protected TabCreatorDelegate m_ReceivingLocCustomSearchTab;

        public ReceivingHeadSearchPage3()
        {
            this.m_ReceivingLocCustomSearchTab = () => StockTranLocation.ReceivingStLocCustomSearchTab.New(StockTranLocation.ReceivingStLocCustomSearchTab.SetupStLocCustom);

            this.Tabs.InsertTabAfter(this.m_ReceivingLocCustomSearchTab, this.m_ReceivingLocSearchTab);
            this.Tabs.RemoveTab(this.m_ReceivingLocSearchTab);
        }

        public override string ACGetTypeFullName()
        {
            return baseType.FullName;
        }

        protected override string GetNamespaceName()
        {
            return baseType.Namespace;
        }
    }
}
