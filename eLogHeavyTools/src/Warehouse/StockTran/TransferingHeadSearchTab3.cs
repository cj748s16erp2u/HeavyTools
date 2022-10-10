using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.Xml;
using System;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadSearchTab3 : TransferingHeadSearchTab, IXmlObjectName
    {
        public override string GetNamespaceName()
        {
            return typeof(TransferingHeadSearchTab).Namespace;
        }
        protected override string GetPageXmlFileName()
        {
            return GetNamespaceName() + "." + typeof(TransferingHeadSearchTab).Name;
        }
        #region IXmlObjectName Members

        private string m_xmlObjectName = null;

        public string XmlObjectName
        {
            get { if (string.IsNullOrEmpty(m_xmlObjectName)) m_xmlObjectName = typeof(TransferingHeadSearchTab).Name; return m_xmlObjectName; }
        }

        #endregion

        protected override void CreateBase()
        {
            base.CreateBase();
            var getfromonroad = new Button("getfromonroad");
            AddCmd(getfromonroad);

            SetButtonAction("getfromonroad", 
                new EditRecordCallbackAction(TransferingOnRoadPage.ID, 
                eEditRecordCallbackFlags.CheckForRootEntityKey), 
                new ControlEvent(OnGetfromonroad));
        }

        private void OnGetfromonroad(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 0)
            {
                args.Continue = true;
                args.AddExecCommand(new StoreToArgsStep(Key.ToJSONKeyArray(SearchResults.SelectedPKS),
                                        Consts.RootEntityKey));
            }
        }
    }
}
