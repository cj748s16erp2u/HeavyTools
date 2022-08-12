using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.BankTran
{
    public class EfxBankTranHeadSearchTab3 : U4Ext.Bank.Base.Transaction.EfxBankTranHeadSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName
        protected static Type baseType = typeof(U4Ext.Bank.Base.Transaction.EfxBankTranHeadSearchTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName
        {
            get { return baseType.Name; }
        }
        #endregion

        protected override void CreateBase()
        {
            base.CreateBase();

            OnPageActivate += EfxBankTranHeadSearchTab3_OnPageActivate;
        }

        private void EfxBankTranHeadSearchTab3_OnPageActivate(PageUpdateArgs args)
        {
            if (args.PageData != null && args.PageData.ContainsKey(Consts.DetailEntityKey))
            {
                if (!SearchResults.ForceRefresh)
                    SearchResults.ForceRefresh = true;
            }
        }

    }
}
