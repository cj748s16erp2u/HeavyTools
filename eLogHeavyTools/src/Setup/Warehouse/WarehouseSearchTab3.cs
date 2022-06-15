using eLog.Base.Setup.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class WarehouseSearchTab3 : WarehouseSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(WarehouseSearchTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName => baseType.Name;

        #endregion

        protected override void CreateDefaultEvents()
        {
            base.CreateDefaultEvents();

            this.OnActivate.AddStep(new eProjectWeb.Framework.UI.Script.ClearEntityKeyStep(eProjectWeb.Framework.Consts.DetailEntityKey));
        }
    }
}
