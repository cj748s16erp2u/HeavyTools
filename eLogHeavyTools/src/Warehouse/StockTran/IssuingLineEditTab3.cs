using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class IssuingLineEditTab3 : IssuingLineEditTab, IXmlObjectName
    {
        public override string GetNamespaceName()
        {
            return typeof(IssuingLineEditTab).Namespace;
        }
        protected override string GetPageXmlFileName()
        {
            return GetNamespaceName() + "." + typeof(IssuingLineEditTab).Name;
        }
        #region IXmlObjectName Members

        private string m_xmlObjectName = null;

        public string XmlObjectName
        {
            get { if (string.IsNullOrEmpty(m_xmlObjectName)) m_xmlObjectName = typeof(IssuingLineEditTab).Name; return m_xmlObjectName; }
        }

        #endregion
        protected override StLine DefaultPageLoad(PageUpdateArgs args)
        {
            var stl= base.DefaultPageLoad(args);
            if (stl != null)
            {
                if (stl.Sordlineid.HasValue)
                { 
                    nbDispQty.Disabled = true;
                    nbMovQty.Disabled = true;
                    nbDispQty2.Disabled = true;
                    nbMovQty2.Disabled = true;
                }
            } 
            return stl;
        }
    }
}
