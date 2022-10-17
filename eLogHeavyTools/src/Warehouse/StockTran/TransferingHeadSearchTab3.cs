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
            if (SearchResults.SelectedPKS.Count == 1)
            { 
                StHead s = StHead.CreateNew();

                string sql = string.Format("select * from {0} where {1} = {2} and {3} < {4} ",
                        StHead._TableName,
                        StHead.FieldStid.Name,
                        Utils.SqlToString(SearchResults.SelectedPK[StHead.FieldStid.Name]),
                        StHead.FieldStstat.Name,
                        Utils.SqlToString((int)StHeadStStatList.Values.Closed));
                try
                {
                    SqlDataAdapter.FillSingleRow(DB.Main, s, sql);

                    if (s != null)
                    {
                        throw new MessageException("$transit_not_closed", StringN.ConvertToString(s.Docnum));
                    }
                }
                catch (RecordNotFoundException)
                {

                }



                args.Continue = true;
                args.AddExecCommand(new StoreToArgsStep(SearchResults.SelectedPK.ToJSON(),
                                    Consts.RootEntityKey));
            }
        }
    }
}
